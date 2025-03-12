using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RepasoAPI.Data;
using RepasoAPI.Models.Entity;
using RepasoAPI.Repository.IRepository;

namespace RepasoAPI.Repository
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string ProductoCacheKey = "ProductoCacheKey"; //cambiadmelo lokos
        private readonly int CacheExpirationTime = 3600;
        public ProductoRepository(ApplicationDbContext context, IMemoryCache cache)
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
            _cache.Remove(ProductoCacheKey);
        }

        public async Task<ICollection<Producto>> GetAllAsync()
        {
            if (_cache.TryGetValue(ProductoCacheKey, out ICollection<Producto> EditorialsCached))
                return EditorialsCached;

            var EditorialsFromDb = await _context.Productos.OrderBy(c => c.Nombre).ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(ProductoCacheKey, EditorialsFromDb, cacheEntryOptions);
            return EditorialsFromDb;
        }

        public async Task<Producto> GetAsync(int id)
        {
            if (_cache.TryGetValue(ProductoCacheKey, out ICollection<Producto> EditorialsCached))
            {
                var EditorialEntity = EditorialsCached.FirstOrDefault(c => c.Id == id);
                if (EditorialEntity != null)
                    return EditorialEntity;
            }

            return await _context.Productos.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Productos.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateAsync(Producto producto)
        {
            await _context.Productos.AddAsync(producto);
            return await Save();
        }

        public async Task<bool> UpdateAsync(Producto producto)
        {
            _context.Update(producto);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var producto = await GetAsync(id);
            if (producto == null)
                return false;

            _context.Productos.Remove(producto);
            return await Save();
        }
    }
}
