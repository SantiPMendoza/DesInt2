using AtecaAPI.Data;
using AtecaAPI.Models.Entity;
using AtecaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AtecaAPI.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="AtecaAPI.Repository.IRepository.IDiaNoLectivoRepository" />
    public class DiaNoLectivoRepository : IDiaNoLectivoRepository
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// The cache
        /// </summary>
        private readonly IMemoryCache _cache;
        /// <summary>
        /// The cache key
        /// </summary>
        private readonly string CacheKey = "DiaNoLectivoCacheKey";
        /// <summary>
        /// The cache expiration time
        /// </summary>
        private readonly int CacheExpirationTime = 3600; // segundos

        /// <summary>
        /// Initializes a new instance of the <see cref="DiaNoLectivoRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="cache">The cache.</param>
        public DiaNoLectivoRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Existses the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.DiasNoLectivos.AnyAsync(d => d.Id == id);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="dia">The dia.</param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(DiaNoLectivo dia)
        {
            await _context.DiasNoLectivos.AddAsync(dia);
            return await Save();
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="dia">The dia.</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(DiaNoLectivo dia)
        {
            _context.DiasNoLectivos.Update(dia);
            return await Save();
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var dia = await GetAsync(id);
            if (dia == null)
                return false;

            _context.DiasNoLectivos.Remove(dia);
            return await Save();
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Save()
        {
            var result = await _context.SaveChangesAsync() >= 0;
            if (result)
                ClearCache();
            return result;
        }


        /// <summary>
        /// Existses the by fecha asynchronous.
        /// </summary>
        /// <param name="fecha">The fecha.</param>
        /// <returns></returns>
        public async Task<bool> ExistsByFechaAsync(DateOnly fecha)
        {
            if (_cache.TryGetValue(CacheKey, out ICollection<DiaNoLectivo> cachedList))
                return cachedList.Any(d => d.Fecha == fecha);

            return await _context.DiasNoLectivos.AnyAsync(d => d.Fecha == fecha);
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public void ClearCache()
        {
            _cache.Remove(CacheKey);
        }
    }
}
