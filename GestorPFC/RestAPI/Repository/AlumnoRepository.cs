using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RestAPI.Models.Entity;
using RestAPI.Data;
using RestAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Repository
{
    public class AlumnoRepository : IAlumnoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        private readonly string _alumnoCacheKey = "AlumnoCacheKey";
        private readonly int _cacheExpirationTime = 3600;

        public AlumnoRepository(ApplicationDbContext context, IMemoryCache cache)
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
            _cache.Remove(_alumnoCacheKey);
        }

        public async Task<ICollection<Alumno>> GetAllAsync()
        {
            if (_cache.TryGetValue(_alumnoCacheKey, out ICollection<Alumno> alumnosCached))
                return alumnosCached;

            var alumnosFromDb = await _context.Alumnos
                .OrderBy(a => a.Nombre)
                .ToListAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheExpirationTime));

            _cache.Set(_alumnoCacheKey, alumnosFromDb, cacheEntryOptions);
            return alumnosFromDb;
        }

        public async Task<Alumno> GetAsync(int id)
        {
            if (_cache.TryGetValue(_alumnoCacheKey, out ICollection<Alumno> alumnosCached))
            {
                var alumno = alumnosCached.FirstOrDefault(a => a.Id == id);
                if (alumno != null)
                    return alumno;
            }
            return await _context.Alumnos.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Alumnos.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> CreateAsync(Alumno alumno)
        {
            await _context.Alumnos.AddAsync(alumno);
            return await Save();
        }

        public async Task<bool> UpdateAsync(Alumno alumno)
        {
            _context.Alumnos.Update(alumno);
            return await Save();
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var alumno = await GetAsync(id);
            if (alumno == null)
                return false;

            _context.Alumnos.Remove(alumno);
            return await Save();
        }
    }
}
