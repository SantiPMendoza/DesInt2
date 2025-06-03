using AtecaAPI.Data;
using AtecaAPI.Models.Entity;
using AtecaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using AtecaAPI.Services;

namespace AtecaAPI.Repository
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string ReservaCacheKey = "ReservaCacheKey";
        private readonly int CacheExpirationTime = 3600;
        private readonly IDiaNoLectivoRepository _diaNoLectivoRepository;
        private readonly INotificationService _notificationService; // NUEVO

        public ReservaRepository(
            ApplicationDbContext context,
            IMemoryCache cache,
            IDiaNoLectivoRepository diaNoLectivoRepository,
            INotificationService notificacionService) // NUEVO
        {
            _context = context;
            _cache = cache;
            _diaNoLectivoRepository = diaNoLectivoRepository;
            _notificationService = notificacionService; // NUEVO
        }

        // Obtener todas las reservas (con cache)
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

        // Obtener una reserva por ID (consulta directa o desde caché)
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

        // Obtener reservas de un profesor
        public async Task<ICollection<Reserva>> GetByProfesorIdAsync(int profesorId) =>
            await _context.Reservas
                .Include(r => r.Profesor)
                .Include(r => r.GrupoClase)
                .Include(r => r.FranjaHoraria)
                .Where(r => r.ProfesorId == profesorId)
                .ToListAsync();

        // Obtener reservas pendientes
        public async Task<ICollection<Reserva>> GetPendientesAsync() =>
            await _context.Reservas
                .Include(r => r.Profesor)
                .Include(r => r.GrupoClase)
                .Include(r => r.FranjaHoraria)
                .Where(r => r.Estado == "Pendiente")
                .ToListAsync();

        // Obtener reservas aprobadas
        public async Task<ICollection<Reserva>> GetAprobadasAsync() =>
            await _context.Reservas
                .Include(r => r.Profesor)
                .Include(r => r.GrupoClase)
                .Include(r => r.FranjaHoraria)
                .Where(r => r.Estado == "Aprobada")
                .ToListAsync();

        // Verificar si existe una reserva por ID
        public async Task<bool> ExistsAsync(int id) =>
            await _context.Reservas.AnyAsync(r => r.Id == id);

        // Crear una reserva
        public async Task<bool> CreateAsync(Reserva reserva)
        {
            await _context.Reservas.AddAsync(reserva);
            return await Save();
        }

        // Actualizar una reserva
        public async Task<bool> UpdateAsync(Reserva reserva)
        {
            _context.Reservas.Update(reserva);
            return await Save();
        }

        // Acepta una reserva, cambia estado y notifica por correo
        public async Task<bool> AceptarReservaAsync(int id)
        {
            var reserva = await GetAsync(id);
            if (reserva == null) return false;

            reserva.Estado = "Aprobada";
            reserva.FechaResolucion = DateTime.Now;

            var result = await UpdateAsync(reserva);
            if (result)
                await _notificationService.EnviarCorreoAsync(
                    reserva.Profesor.Email,
                    "Reserva aprobada",
                    $"<p>Tu reserva del día <strong>{reserva.Fecha}</strong> ha sido <strong>aprobada</strong>.</p>"
                );

            return result;
        }

        // Rechaza una reserva y notifica por correo
        public async Task<bool> RechazarReservaAsync(int id)
        {
            var reserva = await GetAsync(id);
            if (reserva == null) return false;

            reserva.Estado = "Rechazada";
            reserva.FechaResolucion = DateTime.Now;

            var result = await UpdateAsync(reserva);
            if (result)
                await _notificationService.EnviarCorreoAsync(
                    reserva.Profesor.Email,
                    "Reserva rechazada",
                    $"<p>Tu reserva del día <strong>{reserva.Fecha}</strong> ha sido <strong>rechazada</strong>.</p>"
                );

            return result;
        }

        // Eliminar una reserva
        public async Task<bool> DeleteAsync(int id)
        {
            var reserva = await GetAsync(id);
            if (reserva == null) return false;

            _context.Reservas.Remove(reserva);
            return await Save();
        }

        // Guardar cambios y limpiar caché
        public async Task<bool> Save()
        {
            var result = await _context.SaveChangesAsync() >= 0;
            if (result)
                ClearCache();
            return result;
        }

        // Validar condiciones para permitir una reserva
        public async Task<string?> ValidarReservaAsync(Reserva reserva)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            if (await _diaNoLectivoRepository.ExistsByFechaAsync(reserva.Fecha))
                return "No se puede realizar una reserva en un día no lectivo.";

            if (reserva.Fecha < today)
                return "No se puede hacer una reserva para una fecha pasada.";

            var yaExiste = await _context.Reservas.AnyAsync(r =>
                r.Fecha == reserva.Fecha &&
                r.FranjaHorariaId == reserva.FranjaHorariaId &&
                r.ProfesorId == reserva.ProfesorId &&
                r.Estado != "Rechazada" &&
                r.Id != reserva.Id);

            if (yaExiste)
                return "Ya existe una reserva para ese profesor en la fecha y franja horaria seleccionada.";

            if (reserva.Fecha == today)
            {
                var franja = await _context.FranjasHorarias.FindAsync(reserva.FranjaHorariaId);
                if (franja != null && franja.HoraInicio <= TimeOnly.FromDateTime(DateTime.Now))
                    return "No se puede hacer una reserva en una franja horaria ya vencida.";
            }

            return null;
        }

        // Limpiar caché de reservas
        public void ClearCache()
        {
            _cache.Remove(ReservaCacheKey);
        }
    }
}
