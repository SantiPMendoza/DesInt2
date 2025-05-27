using AtecaAPI.Models.Entity;

namespace AtecaAPI.Repository.IRepository
{
    public interface IReservaRepository : IRepository<Reserva>
    {
        Task<ICollection<Reserva>> GetByProfesorIdAsync(int profesorId);
        Task<ICollection<Reserva>> GetPendientesAsync();
    }
}
