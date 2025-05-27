using AtecaAPI.Data;
using AtecaAPI.Models.Entity;
using AtecaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AtecaAPI.Repository
{
    public class FranjaHorariaRepository : IFranjaHorariaRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string FranjaCacheKey = "FranjaHorariaCacheKey";
        private readonly int CacheExpirationTime = 3600; // en segundos

        public FranjaHorariaRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<ICollection<FranjaHoraria>> GetAllAsync()
        {
            if (_cache.TryGetValue(FranjaCacheKey, out ICollection<FranjaHoraria> cachedFranjas))
                return cachedFranjas;

            var franjas = await _context.FranjasHorarias.OrderBy(f => f.Id).ToListAsync();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(FranjaCacheKey, franjas, cacheOptions);
            return franjas;
        }

        public async Task<FranjaHoraria> GetAsync(int id)
        {
            if (_cache.TryGetValue(FranjaCacheKey, out ICollection<FranjaHoraria> cachedFranjas))
            {
                var franja = cachedFranjas.FirstOrDefault(f => f.Id == id);
                if (franja != null)
                    return franja;
            }

            return await _context.FranjasHorarias.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<ICollection<FranjaHoraria>> GetByDiaSemanaAsync(DayOfWeek diaSemana)
        {
            return await _context.FranjasHorarias
                .Where(f => f.DiaSemana == diaSemana)
                .OrderBy(f => f.HoraInicio)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.FranjasHorarias.AnyAsync(f => f.Id == id);
        }

        public async Task<bool> CreateAsync(FranjaHoraria franja)
        {
            await _context.FranjasHorarias.AddAsync(franja);
            return await Save();
        }

        public async Task<bool> UpdateAsync(FranjaHoraria franja)
        {
            _context.FranjasHorarias.Update(franja);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var franja = await GetAsync(id);
            if (franja == null)
                return false;

            _context.FranjasHorarias.Remove(franja);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var result = await _context.SaveChangesAsync() >= 0;
            if (result)
                ClearCache();
            return result;
        }

        public void ClearCache()
        {
            _cache.Remove(FranjaCacheKey);
        }
    }
}
