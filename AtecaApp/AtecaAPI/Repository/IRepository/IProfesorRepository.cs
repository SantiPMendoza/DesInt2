using AtecaAPI.Models.Entity;

namespace AtecaAPI.Repository.IRepository
{
    public interface IProfesorRepository : IRepository<Profesor>
    {
        Task<Profesor?> GetByGoogleIdAsync(string googleId);

        Task<bool> CreateIfNotExistsAsync(Profesor profesor)
    }
}
