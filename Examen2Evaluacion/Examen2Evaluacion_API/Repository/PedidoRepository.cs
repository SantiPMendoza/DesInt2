using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Examen2Evaluacion_API.Data;
using Examen2Evaluacion_API.Models.Entity;
using Examen2Evaluacion_API.Repository.IRepository;

namespace Examen2Evaluacion_API.Repository
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
            if (_cache.TryGetValue(PedidoCacheKey, out ICollection<Pedido> cached))
                return cached;

            var pedidos = await _context.Pedidos
                .Include(p => p.Productos)
                .Include(p => p.Usuario)
                .OrderBy(p => p.Id)
                .ToListAsync();

            _cache.Set(PedidoCacheKey, pedidos, new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(CacheExpirationTime)
            });

            return pedidos;
        }

        public async Task<Pedido> GetAsync(int id)
        {
            return await _context.Pedidos
                .Include(p => p.Productos)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id);
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

