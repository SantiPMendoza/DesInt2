using AtecaAPI.Models.Entity;

namespace AtecaAPI.Repository.IRepository
{
    public interface IDisponibilidadAulaRepository : IRepository<DisponibilidadAula>
    {
        Task<ICollection<DisponibilidadAula>> GetByDiaSemanaAsync(DayOfWeek diaSemana);
    }
}