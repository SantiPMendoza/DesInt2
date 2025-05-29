using AtecaAPI.Models.Entity;
using AtecaAPI.Repository.IRepository;

public interface IReservaRepository : IRepository<Reserva>
{
    Task<ICollection<Reserva>> GetByProfesorIdAsync(int profesorId);
    Task<ICollection<Reserva>> GetPendientesAsync();

    // Añade esto:
    Task<bool> AceptarReservaAsync(int id);
    Task<bool> RechazarReservaAsync(int id);
}
