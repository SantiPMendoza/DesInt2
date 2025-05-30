using AtecaAPI.Data;
using AtecaAPI.Models.Entity;
using AtecaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AtecaAPI.Repository
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string ReservaCacheKey = "ReservaCacheKey";
        private readonly int CacheExpirationTime = 3600; // en segundos => posiblemente reducir en un futuro

        public ReservaRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<ICollection<Reserva>> GetAllAsync()
        {
            if (_cache.TryGetValue(ReservaCacheKey, out ICollection<Reserva> reservasCached))
                return reservasCached;

            var reservasFromDb = await _context.Reservas
                .Include(r => r.Profesor)
                .Include(r => r.GrupoClase)
                .Include(r => r.FranjaHoraria)
                .ToListAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(ReservaCacheKey, reservasFromDb, cacheEntryOptions);

            return reservasFromDb;
        }

        public async Task<Reserva> GetAsync(int id)
        {
            if (_cache.TryGetValue(ReservaCacheKey, out ICollection<Reserva> reservasCached))
            {
                var reserva = reservasCached.FirstOrDefault(r => r.Id == id);
                if (reserva != null)
                    return reserva;
            }

            return await _context.Reservas
                .Include(r => r.Profesor)
                .Include(r => r.GrupoClase)
                .Include(r => r.FranjaHoraria)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ICollection<Reserva>> GetByProfesorIdAsync(int profesorId)
            => await _context.Reservas
                .Include(r => r.Profesor)
                .Include(r => r.GrupoClase)
                .Include(r => r.FranjaHoraria)
                .Where(r => r.ProfesorId == profesorId)
                .ToListAsync();

        public async Task<ICollection<Reserva>> GetPendientesAsync()
            => await _context.Reservas
                .Include(r => r.Profesor)
                .Include(r => r.GrupoClase)
                .Include(r => r.FranjaHoraria)
                .Where(r => r.Estado == "Pendiente")
                .ToListAsync();

        public async Task<bool> ExistsAsync(int id)
            => await _context.Reservas.AnyAsync(r => r.Id == id);

        public async Task<bool> CreateAsync(Reserva reserva)
        {
            await _context.Reservas.AddAsync(reserva);
            return await Save();
        }

        public async Task<bool> UpdateAsync(Reserva reserva)
        {
            _context.Reservas.Update(reserva);
            return await Save();
        }

        public async Task<bool> AceptarReservaAsync(int id)
        {
            var reserva = await GetAsync(id);
            if (reserva == null) return false;

            reserva.Estado = "Aceptada";
            reserva.FechaResolucion = DateTime.Now;
            return await UpdateAsync(reserva);
        }

        public async Task<bool> RechazarReservaAsync(int id)
        {
            var reserva = await GetAsync(id);
            if (reserva == null) return false;

            reserva.Estado = "Rechazada";
            reserva.FechaResolucion = DateTime.Now;
            return await UpdateAsync(reserva);
        }



        public async Task<bool> DeleteAsync(int id)
        {
            var reserva = await GetAsync(id);
            if (reserva == null) return false;

            _context.Reservas.Remove(reserva);
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
            _cache.Remove(ReservaCacheKey);
        }
    }
}
