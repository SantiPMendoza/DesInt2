using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ExamenAPI.Data;
using ExamenAPI.Models.Entity;
using ExamenAPI.Repository.IRepository;

namespace ExamenAPI.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string StudentCacheKey = "StudentCacheKey";
        private readonly int CacheExpirationTime = 3600;

        public StudentRepository(ApplicationDbContext context, IMemoryCache cache)
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
            _cache.Remove(StudentCacheKey);
        }

        public async Task<ICollection<Student>> GetAllAsync()
        {
            if (_cache.TryGetValue(StudentCacheKey, out ICollection<Student> cachedStudents))
                return cachedStudents;

            var studentsFromDb = await _context.Students
                .Include(s => s.Courses)
                .OrderBy(s => s.Id)
                .ToListAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(StudentCacheKey, studentsFromDb, cacheEntryOptions);
            return studentsFromDb;
        }

        public async Task<Student> GetAsync(int id)
        {
            if (_cache.TryGetValue(StudentCacheKey, out ICollection<Student> cachedStudents))
            {
                var student = cachedStudents.FirstOrDefault(s => s.Id == id);
                if (student != null)
                    return student;
            }

            return await _context.Students
                .Include(s => s.Courses)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Students.AnyAsync(s => s.Id == id);
        }

        public async Task<bool> CreateAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            return await Save();
        }

        public async Task<bool> UpdateAsync(Student student)
        {
            _context.Students.Update(student);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await GetAsync(id);
            if (student == null)
                return false;

            _context.Students.Remove(student);
            return await Save();
        }
    }
}

