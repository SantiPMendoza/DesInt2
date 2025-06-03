using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using AtecaAPI.Data;
using AtecaAPI.Models.Entity;
using AtecaAPI.Repository.IRepository;

namespace AtecaAPI.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="AtecaAPI.Repository.IRepository.IGrupoClaseRepository" />
    public class GrupoClaseRepository : IGrupoClaseRepository
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
        /// The grupo clase cache key
        /// </summary>
        private readonly string GrupoClaseCacheKey = "GrupoClaseCacheKey"; //cambiadmelo lokos
        /// <summary>
        /// The cache expiration time
        /// </summary>
        private readonly int CacheExpirationTime = 3600;
        /// <summary>
        /// Initializes a new instance of the <see cref="GrupoClaseRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="cache">The cache.</param>
        public GrupoClaseRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Save()
        {
            var result = await _context.SaveChangesAsync() >= 0;
            if (result)
            {
                ClearCache();
            }
            return result;
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public void ClearCache()
        {
            _cache.Remove(GrupoClaseCacheKey);
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Existses the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.GruposClase.AnyAsync(c => c.Id == id);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="grupoClase">The grupo clase.</param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(GrupoClase grupoClase)
        {
            await _context.GruposClase.AddAsync(grupoClase);
            return await Save();
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="grupoClase">The grupo clase.</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(GrupoClase grupoClase)
        {
            _context.Update(grupoClase);
            return await Save();
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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

