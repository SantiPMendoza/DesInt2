using ExamenApi.Data;
using ExamenApi.Models;
using ExamenApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ExamenApi.Repository
{
    public class JuegoRepository : IJuegoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        private readonly string _juegoCacheKey = "JuegoCacheKey";
        private readonly int _cacheExpirationTime = 3600;

        public JuegoRepository(ApplicationDbContext context, IMemoryCache cache)
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
            _cache.Remove(_juegoCacheKey);
        }

        public async Task<ICollection<Juego>> GetAllAsync()
        {
            if (_cache.TryGetValue(_juegoCacheKey, out ICollection<Juego> juegosCached))
                return juegosCached;

            var proyectosFromDb = await _context.Juegos
                .OrderBy(p => p.Name)
                .ToListAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheExpirationTime));

            _cache.Set(_juegoCacheKey, proyectosFromDb, cacheEntryOptions);
            return proyectosFromDb;
        }

        public async Task<Juego> GetAsync(int id)
        {
            if (_cache.TryGetValue(_juegoCacheKey, out ICollection<Juego> juegosCached))
            {
                var juego = juegosCached.FirstOrDefault(p => p.Id == id);
                if (juego != null)
                    return juego;
            }
            return await _context.Juegos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Juegos.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> CreateAsync(Juego juego)
        {
            await _context.Juegos.AddAsync(juego);
            return await Save();
        }

        public async Task<bool> UpdateAsync(Juego juego)
        {
            _context.Juegos.Update(juego);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var juego = await GetAsync(id);
            if (juego == null)
                return false;

            _context.Juegos.Remove(juego);
            return await Save();
        }

        public async Task<IEnumerable<Juego>> GetTop10Async()
        {
            return await _context.Juegos
                .OrderByDescending(j => j.Resultado)
                .Take(10)
                .ToListAsync();
        }

    }
}