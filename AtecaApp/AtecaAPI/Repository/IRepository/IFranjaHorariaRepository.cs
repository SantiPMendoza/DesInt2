using AtecaAPI.Models.Entity;

namespace AtecaAPI.Repository.IRepository
{
    public interface IFranjaHorariaRepository : IRepository<FranjaHoraria>
    {
        Task<ICollection<FranjaHoraria>> GetByDiaSemanaAsync(DayOfWeek diaSemana);
    }
}