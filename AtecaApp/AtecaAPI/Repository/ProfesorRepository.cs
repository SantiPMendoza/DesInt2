using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using AtecaAPI.Data;
using AtecaAPI.Models.Entity;
using AtecaAPI.Repository.IRepository;

namespace AtecaAPI.Repository
{
    public class ProfesorRepository : IProfesorRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string ProfesorCacheKey = "ProfesorCacheKey"; //cambiadmelo lokos
        private readonly int CacheExpirationTime = 3600;
        public ProfesorRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<bool> Save()
        {
            var result = await _context.SaveChangesAsync() >= 0;
            if (result)
            {
                ClearCache();
            }
            return result;
        }

        public void ClearCache()
        {
            _cache.Remove(ProfesorCacheKey);
        }

        public async Task<ICollection<Profesor>> GetAllAsync()
        {
            if (_cache.TryGetValue(ProfesorCacheKey, out ICollection<Profesor> ProfesorCached))
                return ProfesorCached;

            var ProfesoresFromDb = await _context.Profesores.OrderBy(c => c.Id).ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(ProfesorCacheKey, ProfesoresFromDb, cacheEntryOptions);
            return ProfesoresFromDb;
        }

        public async Task<Profesor> GetAsync(int id)
        {
            if (_cache.TryGetValue(ProfesorCacheKey, out ICollection<Profesor> ProfesorCached))
            {
                var Profesor = ProfesorCached.FirstOrDefault(c => c.Id == id);
                if (Profesor != null)
                    return Profesor;
            }

            return await _context.Profesores.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Profesor?> GetByGoogleIdAsync(string googleId)
        {
            return await _context.Profesores.FirstOrDefaultAsync(p => p.GoogleId == googleId);
        }


        // Crea un nuevo profesor solo si no existe otro con mismo GoogleId o Email
        public async Task<bool> CreateIfNotExistsAsync(Profesor profesor)
        {
            var exists = await _context.Profesores.AnyAsync(p => p.GoogleId == profesor.GoogleId || p.Email == profesor.Email);
            if (exists) return false;

            await _context.Profesores.AddAsync(profesor);
            return await Save();
        }





        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Profesores.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateAsync(Profesor profesor)
        {
            await _context.Profesores.AddAsync(profesor);
            return await Save();
        }

        public async Task<bool> UpdateAsync(Profesor profesor)
        {
            _context.Update(profesor);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var profesor = await GetAsync(id);
            if (profesor == null)
                return false;

            _context.Profesores.Remove(profesor);
            return await Save();
        }
    }
}

