using AtecaAPI.Data;
using AtecaAPI.Models.Entity;
using AtecaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AtecaAPI.Repository
{
    public class DiaNoLectivoRepository : IDiaNoLectivoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string CacheKey = "DiaNoLectivoCacheKey";
        private readonly int CacheExpirationTime = 3600; // segundos

        public DiaNoLectivoRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<ICollection<DiaNoLectivo>> GetAllAsync()
        {
            if (_cache.TryGetValue(CacheKey, out ICollection<DiaNoLectivo> cachedList))
                return cachedList;

            var listFromDb = await _context.DiasNoLectivos.OrderBy(d => d.Fecha).ToListAsync();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(CacheKey, listFromDb, cacheOptions);
            return listFromDb;
        }

        public async Task<DiaNoLectivo> GetAsync(int id)
        {
            if (_cache.TryGetValue(CacheKey, out ICollection<DiaNoLectivo> cachedList))
            {
                var item = cachedList.FirstOrDefault(d => d.Id == id);
                if (item != null)
                    return item;
            }

            return await _context.DiasNoLectivos.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.DiasNoLectivos.AnyAsync(d => d.Id == id);
        }

        public async Task<bool> CreateAsync(DiaNoLectivo dia)
        {
            await _context.DiasNoLectivos.AddAsync(dia);
            return await Save();
        }

        public async Task<bool> UpdateAsync(DiaNoLectivo dia)
        {
            _context.DiasNoLectivos.Update(dia);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var dia = await GetAsync(id);
            if (dia == null)
                return false;

            _context.DiasNoLectivos.Remove(dia);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var result = await _context.SaveChangesAsync() >= 0;
            if (result)
                ClearCache();
            return result;
        }


        public async Task<bool> ExistsByFechaAsync(DateOnly fecha)
        {
            if (_cache.TryGetValue(CacheKey, out ICollection<DiaNoLectivo> cachedList))
                return cachedList.Any(d => d.Fecha == fecha);

            return await _context.DiasNoLectivos.AnyAsync(d => d.Fecha == fecha);
        }

        public void ClearCache()
        {
            _cache.Remove(CacheKey);
        }
    }
}
