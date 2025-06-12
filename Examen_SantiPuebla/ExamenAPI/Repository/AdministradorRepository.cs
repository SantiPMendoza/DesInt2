using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ExamenAPI.Data;
using ExamenAPI.Models.Entity;
using ExamenAPI.Repository.IRepository;

namespace AtecaAPI.Repository
{
    public class AdministradorRepository : IAdministradorRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string AdministradorCacheKey = "AdministradorCacheKey"; //cambiadmelo lokos
        private readonly int CacheExpirationTime = 3600;
        public AdministradorRepository(ApplicationDbContext context, IMemoryCache cache)
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
            _cache.Remove(AdministradorCacheKey);
        }

        public async Task<ICollection<Administrador>> GetAllAsync()
        {
            if (_cache.TryGetValue(AdministradorCacheKey, out ICollection<Administrador> AdministradorCached))
                return AdministradorCached;

            var AdministradoresFromDb = await _context.Administradores.OrderBy(c => c.Id).ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(AdministradorCacheKey, AdministradoresFromDb, cacheEntryOptions);
            return AdministradoresFromDb;
        }

        public async Task<Administrador> GetAsync(int id)
        {
            if (_cache.TryGetValue(AdministradorCacheKey, out ICollection<Administrador> ProfesorCached))
            {
                var Administrador = ProfesorCached.FirstOrDefault(c => c.Id == id);
                if (Administrador != null)
                    return Administrador;
            }

            return await _context.Administradores.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Administradores.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateAsync(Administrador administrador)
        {
            await _context.Administradores.AddAsync(administrador);
            return await Save();
        }

        public async Task<bool> UpdateAsync(Administrador administrador)
        {
            _context.Update(administrador);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var administrador = await GetAsync(id);
            if (administrador == null)
                return false;

            _context.Administradores.Remove(administrador);
            return await Save();
        }
    }
}

