using RestAPI.Data;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Repository
{
    public class ProyectoRepository : IProyectoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        private readonly string _proyectoCacheKey = "ProyectoCacheKey";
        private readonly int _cacheExpirationTime = 3600;

        public ProyectoRepository(ApplicationDbContext context, IMemoryCache cache)
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
            _cache.Remove(_proyectoCacheKey);
        }

        public async Task<ICollection<Proyecto>> GetAllAsync()
        {
            if (_cache.TryGetValue(_proyectoCacheKey, out ICollection<Proyecto> proyectosCached))
                return proyectosCached;

            var proyectosFromDb = await _context.Proyectos
                .OrderBy(p => p.Titulo)
                .ToListAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheExpirationTime));

            _cache.Set(_proyectoCacheKey, proyectosFromDb, cacheEntryOptions);
            return proyectosFromDb;
        }

        public async Task<Proyecto> GetAsync(int id)
        {
            if (_cache.TryGetValue(_proyectoCacheKey, out ICollection<Proyecto> proyectosCached))
            {
                var proyecto = proyectosCached.FirstOrDefault(p => p.Id == id);
                if (proyecto != null)
                    return proyecto;
            }
            return await _context.Proyectos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Proyectos.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> CreateAsync(Proyecto proyecto)
        {
            await _context.Proyectos.AddAsync(proyecto);
            return await Save();
        }

        public async Task<bool> UpdateAsync(Proyecto proyecto)
        {
            _context.Proyectos.Update(proyecto);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var proyecto = await GetAsync(id);
            if (proyecto == null)
                return false;

            _context.Proyectos.Remove(proyecto);
            return await Save();
        }
    }
}
