using AtecaAPI.Data;
using AtecaAPI.Models.Entity;
using AtecaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace AtecaAPI.Repository
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly ApplicationDbContext _context;
        public ReservaRepository(ApplicationDbContext context) => _context = context;

        public async Task<ICollection<Reserva>> GetAllAsync()
            => await _context.Reservas.Include(r => r.Profesor).Include(r => r.GrupoClase).ToListAsync();

        public async Task<Reserva> GetAsync(int id)
            => await _context.Reservas.Include(r => r.Profesor).Include(r => r.GrupoClase)
                   .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<ICollection<Reserva>> GetByProfesorIdAsync(int profesorId)
            => await _context.Reservas.Where(r => r.ProfesorId == profesorId).ToListAsync();

        public async Task<ICollection<Reserva>> GetPendientesAsync()
            => await _context.Reservas.Where(r => r.Estado == "Pendiente").ToListAsync();

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

        public async Task<bool> DeleteAsync(int id)
        {
            var reserva = await GetAsync(id);
            if (reserva == null) return false;
            _context.Reservas.Remove(reserva);
            return await Save();
        }

        public async Task<bool> Save() => await _context.SaveChangesAsync() >= 0;

        public void ClearCache()
        {
            throw new NotImplementedException();
        }
    }
}
