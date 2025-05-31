using AtecaAPI.Models.Entity;

namespace AtecaAPI.Repository.IRepository
{
    public interface IDiaNoLectivoRepository : IRepository<DiaNoLectivo> {
        Task<bool> ExistsByFechaAsync(DateOnly fecha);
    }
}
