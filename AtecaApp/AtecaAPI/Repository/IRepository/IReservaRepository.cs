using AtecaAPI.Models.Entity;
using AtecaAPI.Repository.IRepository;

public interface IReservaRepository : IRepository<Reserva>
{
    Task<ICollection<Reserva>> GetByProfesorIdAsync(int profesorId);
    Task<ICollection<Reserva>> GetAprobadasAsync();
    Task<ICollection<Reserva>> GetPendientesAsync();
    Task<bool> AceptarReservaAsync(int id);
    Task<bool> RechazarReservaAsync(int id);
    Task<string> ValidarReservaAsync(Reserva reserva);
}
