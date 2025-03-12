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
    public class DepartamentoRepository : IDepartamentoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        private readonly string _departamentoCacheKey = "DepartamentoCacheKey";
        private readonly int _cacheExpirationTime = 3600; 

        public DepartamentoRepository(ApplicationDbContext context, IMemoryCache cache)
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
            _cache.Remove(_departamentoCacheKey);
        }

        public async Task<ICollection<Departamento>> GetAllAsync()
        {
            if (_cache.TryGetValue(_departamentoCacheKey, out ICollection<Departamento> departamentosCached))
                return departamentosCached;

            var departamentosFromDb = await _context.Departamentos
                .OrderBy(d => d.Nombre)
                .ToListAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheExpirationTime));

            _cache.Set(_departamentoCacheKey, departamentosFromDb, cacheEntryOptions);
            return departamentosFromDb;
        }

        public async Task<Departamento> GetAsync(int id)
        {
            if (_cache.TryGetValue(_departamentoCacheKey, out ICollection<Departamento> departamentosCached))
            {
                var departamento = departamentosCached.FirstOrDefault(d => d.Id == id);
                if (departamento != null)
                    return departamento;
            }
            return await _context.Departamentos.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Departamentos.AnyAsync(d => d.Id == id);
        }

        public async Task<bool> CreateAsync(Departamento departamento)
        {
            await _context.Departamentos.AddAsync(departamento);
            return await Save();
        }

        public async Task<bool> UpdateAsync(Departamento departamento)
        {
            _context.Departamentos.Update(departamento);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var departamento = await GetAsync(id);
            if (departamento == null)
                return false;

            _context.Departamentos.Remove(departamento);
            return await Save();
        }
    }
}
