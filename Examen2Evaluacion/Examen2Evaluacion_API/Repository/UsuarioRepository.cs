using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Examen2Evaluacion_API.Data;
using Examen2Evaluacion_API.Models.Entity;
using Examen2Evaluacion_API.Repository.IRepository;

namespace Examen2Evaluacion_API.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string UsuarioCacheKey = "UsuarioCacheKey"; //cambiadmelo lokos
        private readonly int CacheExpirationTime = 3600;
        public UsuarioRepository(ApplicationDbContext context, IMemoryCache cache)
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
            _cache.Remove(UsuarioCacheKey);
        }

        public async Task<ICollection<Usuario>> GetAllAsync()
        {
            if (_cache.TryGetValue(UsuarioCacheKey, out ICollection<Usuario> UsuarioCached))
                return UsuarioCached;

            var EditorialsFromDb = await _context.Usuarios.OrderBy(c => c.Id).ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(UsuarioCacheKey, EditorialsFromDb, cacheEntryOptions);
            return EditorialsFromDb;
        }

        public async Task<Usuario> GetAsync(int id)
        {
            if (_cache.TryGetValue(UsuarioCacheKey, out ICollection<Usuario> UsuarioCached))
            {
                var Usuario = UsuarioCached.FirstOrDefault(c => c.Id == id);
                if (Usuario != null)
                    return Usuario;
            }

            return await _context.Usuarios.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Usuarios.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            return await Save();
        }

        public async Task<bool> UpdateAsync(Usuario usuario)
        {
            _context.Update(usuario);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var pedido = await GetAsync(id);
            if (pedido == null)
                return false;

            _context.Usuarios.Remove(pedido);
            return await Save();
        }
    }
}

