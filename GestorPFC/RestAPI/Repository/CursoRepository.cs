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
    public class CursoRepository : ICursoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        private readonly string _cursoCacheKey = "CursoCacheKey";
        private readonly int _cacheExpirationTime = 3600; 

        public CursoRepository(ApplicationDbContext context, IMemoryCache cache)
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
            _cache.Remove(_cursoCacheKey);
        }

        public async Task<ICollection<Curso>> GetAllAsync()
        {
            if (_cache.TryGetValue(_cursoCacheKey, out ICollection<Curso> cursosCached))
                return cursosCached;

            var cursosFromDb = await _context.Cursos
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheExpirationTime));

            _cache.Set(_cursoCacheKey, cursosFromDb, cacheEntryOptions);
            return cursosFromDb;
        }

        public async Task<Curso> GetAsync(int id)
        {
            if (_cache.TryGetValue(_cursoCacheKey, out ICollection<Curso> cursosCached))
            {
                var curso = cursosCached.FirstOrDefault(c => c.Id == id);
                if (curso != null)
                    return curso;
            }
            return await _context.Cursos.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Cursos.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateAsync(Curso curso)
        {
            await _context.Cursos.AddAsync(curso);
            return await Save();
        }

        public async Task<bool> UpdateAsync(Curso curso)
        {
            _context.Cursos.Update(curso);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var curso = await GetAsync(id);
            if (curso == null)
                return false;

            _context.Cursos.Remove(curso);
            return await Save();
        }
    }
}
