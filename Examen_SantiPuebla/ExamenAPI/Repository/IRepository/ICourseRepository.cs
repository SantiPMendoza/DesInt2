using ExamenAPI.Models.Entity;

namespace ExamenAPI.Repository.IRepository
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<List<Student>> LoadStudentsByIds(List<int> ids);
        Task<List<Teacher>> LoadTeachersByIds(List<int> ids);

    }
}
