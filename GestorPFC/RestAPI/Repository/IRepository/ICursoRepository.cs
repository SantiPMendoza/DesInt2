using RestAPI.Models.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;
using RestAPI.Repository.IRepository;

namespace RestAPI.Repository.IRepository
{
    public interface ICursoRepository : IRepository<Curso>
    {
    }
}
