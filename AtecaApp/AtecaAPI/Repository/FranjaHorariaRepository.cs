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
    /// <seealso cref="AtecaAPI.Repository.IRepository.IFranjaHorariaRepository" />
    public class FranjaHorariaRepository : IFranjaHorariaRepository
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
        /// The franja cache key
        /// </summary>
        private readonly string FranjaCacheKey = "FranjaHorariaCacheKey";
        /// <summary>
        /// The cache expiration time
        /// </summary>
        private readonly int CacheExpirationTime = 3600; // en segundos

        /// <summary>
        /// Initializes a new instance of the <see cref="FranjaHorariaRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="cache">The cache.</param>
        public FranjaHorariaRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<FranjaHoraria>> GetAllAsync()
        {
            if (_cache.TryGetValue(FranjaCacheKey, out ICollection<FranjaHoraria> cachedFranjas))
                return cachedFranjas;

            var franjas = await _context.FranjasHorarias.OrderBy(f => f.Id).ToListAsync();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(FranjaCacheKey, franjas, cacheOptions);
            return franjas;
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<FranjaHoraria> GetAsync(int id)
        {
            if (_cache.TryGetValue(FranjaCacheKey, out ICollection<FranjaHoraria> cachedFranjas))
            {
                var franja = cachedFranjas.FirstOrDefault(f => f.Id == id);
                if (franja != null)
                    return franja;
            }

            return await _context.FranjasHorarias.FirstOrDefaultAsync(f => f.Id == id);
        }



        /// <summary>
        /// Existses the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.FranjasHorarias.AnyAsync(f => f.Id == id);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="franja">The franja.</param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(FranjaHoraria franja)
        {
            await _context.FranjasHorarias.AddAsync(franja);
            return await Save();
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="franja">The franja.</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(FranjaHoraria franja)
        {
            _context.FranjasHorarias.Update(franja);
            return await Save();
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var franja = await GetAsync(id);
            if (franja == null)
                return false;

            _context.FranjasHorarias.Remove(franja);
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
        /// Clears the cache.
        /// </summary>
        public void ClearCache()
        {
            _cache.Remove(FranjaCacheKey);
        }
    }
}
