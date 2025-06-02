using ExamenApi.Models;

namespace ExamenApi.Repository.IRepository
{
    public interface IJuegoRepository : IRepository<Juego>
    {
        Task<IEnumerable<Juego>> GetTop10Async();

    }
}
