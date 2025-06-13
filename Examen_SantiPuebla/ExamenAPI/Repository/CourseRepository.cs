using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ExamenAPI.Data;
using ExamenAPI.Models.Entity;
using ExamenAPI.Repository.IRepository;

namespace ExamenAPI.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string CourseCacheKey = "CourseCacheKey";
        private readonly int CacheExpirationTime = 3600;

        public CourseRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<bool> Save()
        {
            var result = await _context.SaveChangesAsync() >= 0;
            if (result)
            {
                ClearCache();
            }
            return result;
        }

        public void ClearCache()
        {
            _cache.Remove(CourseCacheKey);
        }

        public async Task<ICollection<Course>> GetAllAsync()
        {
            if (_cache.TryGetValue(CourseCacheKey, out ICollection<Course> cachedCourses))
                return cachedCourses;

            var coursesFromDb = await _context.Courses
                .Include(c => c.Students)
                .Include(c => c.Teachers)
                .OrderBy(c => c.Id)
                .ToListAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(CourseCacheKey, coursesFromDb, cacheEntryOptions);
            return coursesFromDb;
        }

        public async Task<Course> GetAsync(int id)
        {
            if (_cache.TryGetValue(CourseCacheKey, out ICollection<Course> cachedCourses))
            {
                var course = cachedCourses.FirstOrDefault(c => c.Id == id);
                if (course != null)
                    return course;
            }

            return await _context.Courses
                .Include(c => c.Students)
                .Include(c => c.Teachers)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Courses.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
            return await Save();
        }

        public async Task<bool> UpdateAsync(Course course)
        {
            _context.Courses.Update(course);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await GetAsync(id);
            if (course == null)
                return false;

            _context.Courses.Remove(course);
            return await Save();
        }

        public async Task<List<Student>> LoadStudentsByIds(List<int> ids) =>
    await _context.Students.Where(s => ids.Contains(s.Id)).ToListAsync();

        public async Task<List<Teacher>> LoadTeachersByIds(List<int> ids) =>
            await _context.Teachers.Where(t => ids.Contains(t.Id)).ToListAsync();

    }
}
