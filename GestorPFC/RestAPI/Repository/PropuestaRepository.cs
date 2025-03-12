
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RestAPI.Data;
using RestAPI.Repository.IRepository;
using RestAPI.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Repository
{
    public class PropuestaRepository : IPropuestaRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string _propuestaCacheKey = "PropuestaCacheKey";
        private readonly int _cacheExpirationTime = 3600;

        public PropuestaRepository(ApplicationDbContext context, IMemoryCache cache)
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
            _cache.Remove(_propuestaCacheKey);
        }

        public async Task<ICollection<Propuesta>> GetAllAsync()
        {
            if (_cache.TryGetValue(_propuestaCacheKey, out ICollection<Propuesta> propuestasCached))
                return propuestasCached;

            var propuestasFromDb = await _context.Propuestas
                .OrderBy(p => p.Titulo)
                .ToListAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheExpirationTime));

            _cache.Set(_propuestaCacheKey, propuestasFromDb, cacheEntryOptions);
            return propuestasFromDb;
        }

        public async Task<Propuesta> GetAsync(int id)
        {
            if (_cache.TryGetValue(_propuestaCacheKey, out ICollection<Propuesta> propuestasCached))
            {
                var propuesta = propuestasCached.FirstOrDefault(p => p.Id == id);
                if (propuesta != null)
                    return propuesta;
            }

            return await _context.Propuestas.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Propuestas.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> CreateAsync(Propuesta propuesta)
        {
            await _context.Propuestas.AddAsync(propuesta);
            return await Save();
        }

        public async Task<bool> UpdateAsync(Propuesta propuesta)
        {
            _context.Propuestas.Update(propuesta);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var propuesta = await GetAsync(id);
            if (propuesta == null)
                return false;
            _context.Propuestas.Remove(propuesta);
            return await Save();
        }
    }
}
