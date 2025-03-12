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
    public class ProfesorRepository : IProfesorRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        private readonly string _profesorCacheKey = "ProfesorCacheKey";
        private readonly int _cacheExpirationTime = 3600;

        public ProfesorRepository(ApplicationDbContext context, IMemoryCache cache)
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
            _cache.Remove(_profesorCacheKey);
        }

        public async Task<ICollection<Profesor>> GetAllAsync()
        {
            if (_cache.TryGetValue(_profesorCacheKey, out ICollection<Profesor> profesoresCached))
                return profesoresCached;

            var profesoresFromDb = await _context.Profesores
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheExpirationTime));

            _cache.Set(_profesorCacheKey, profesoresFromDb, cacheEntryOptions);
            return profesoresFromDb;
        }

        public async Task<Profesor> GetAsync(int id)
        {
            if (_cache.TryGetValue(_profesorCacheKey, out ICollection<Profesor> profesoresCached))
            {
                var profesor = profesoresCached.FirstOrDefault(p => p.Id == id);
                if (profesor != null)
                    return profesor;
            }
            return await _context.Profesores.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Profesores.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> CreateAsync(Profesor profesor)
        {
            await _context.Profesores.AddAsync(profesor);
            return await Save();
        }

        public async Task<bool> UpdateAsync(Profesor profesor)
        {
            _context.Profesores.Update(profesor);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var profesor = await GetAsync(id);
            if (profesor == null)
                return false;

            _context.Profesores.Remove(profesor);
            return await Save();
        }
    }
}
