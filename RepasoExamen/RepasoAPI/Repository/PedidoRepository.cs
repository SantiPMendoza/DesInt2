using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RepasoAPI.Data;
using RepasoAPI.Models.Entity;
using RepasoAPI.Repository.IRepository;

namespace RepasoAPI.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string PedidoCacheKey = "PedidoCacheKey"; //cambiadmelo lokos
        private readonly int CacheExpirationTime = 3600;
        public PedidoRepository(ApplicationDbContext context, IMemoryCache cache)
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
            _cache.Remove(PedidoCacheKey);
        }

        public async Task<ICollection<Pedido>> GetAllAsync()
        {
            if (_cache.TryGetValue(PedidoCacheKey, out ICollection<Pedido> EditorialsCached))
                return EditorialsCached;

            var EditorialsFromDb = await _context.Pedidos.OrderBy(c => c.Id).ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(PedidoCacheKey, EditorialsFromDb, cacheEntryOptions);
            return EditorialsFromDb;
        }

        public async Task<Pedido> GetAsync(int id)
        {
            if (_cache.TryGetValue(PedidoCacheKey, out ICollection<Pedido> EditorialsCached))
            {
                var EditorialEntity = EditorialsCached.FirstOrDefault(c => c.Id == id);
                if (EditorialEntity != null)
                    return EditorialEntity;
            }

            return await _context.Pedidos.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Pedidos.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateAsync(Pedido pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
            return await Save();
        }

        public async Task<bool> UpdateAsync(Pedido pedido)
        {
            _context.Update(pedido);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var pedido = await GetAsync(id);
            if (pedido == null)
                return false;

            _context.Pedidos.Remove(pedido);
            return await Save();
        }

    }
}

