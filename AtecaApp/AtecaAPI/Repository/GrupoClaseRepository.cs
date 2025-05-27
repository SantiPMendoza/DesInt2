using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using AtecaAPI.Data;
using AtecaAPI.Models.Entity;
using AtecaAPI.Repository.IRepository;

namespace AtecaAPI.Repository
{
    public class GrupoClaseRepository : IGrupoClaseRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string GrupoClaseCacheKey = "GrupoClaseCacheKey"; //cambiadmelo lokos
        private readonly int CacheExpirationTime = 3600;
        public GrupoClaseRepository(ApplicationDbContext context, IMemoryCache cache)
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
            _cache.Remove(GrupoClaseCacheKey);
        }

        public async Task<ICollection<GrupoClase>> GetAllAsync()
        {
            if (_cache.TryGetValue(GrupoClaseCacheKey, out ICollection<GrupoClase> GrupoClaseCached))
                return GrupoClaseCached;

            var GrupoClaseFromDb = await _context.GruposClase.OrderBy(c => c.Id).ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(GrupoClaseCacheKey, GrupoClaseFromDb, cacheEntryOptions);
            return GrupoClaseFromDb;
        }

        public async Task<GrupoClase> GetAsync(int id)
        {
            if (_cache.TryGetValue(GrupoClaseCacheKey, out ICollection<GrupoClase> GrupoClaseCached))
            {
                var GrupoClase = GrupoClaseCached.FirstOrDefault(c => c.Id == id);
                if (GrupoClase != null)
                    return GrupoClase;
            }

            return await _context.GruposClase.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.GruposClase.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateAsync(GrupoClase grupoClase)
        {
            await _context.GruposClase.AddAsync(grupoClase);
            return await Save();
        }

        public async Task<bool> UpdateAsync(GrupoClase grupoClase)
        {
            _context.Update(grupoClase);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var grupoClase = await GetAsync(id);
            if (grupoClase == null)
                return false;

            _context.GruposClase.Remove(grupoClase);
            return await Save();
        }
    }
}

