using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ExamenAPI.Data;
using ExamenAPI.Models.Entity;
using ExamenAPI.Repository.IRepository;

namespace ExamenAPI.Repository
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string TeacherCacheKey = "TeacherCacheKey";
        private readonly int CacheExpirationTime = 3600;

        public TeacherRepository(ApplicationDbContext context, IMemoryCache cache)
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
            _cache.Remove(TeacherCacheKey);
        }

        public async Task<ICollection<Teacher>> GetAllAsync()
        {
            if (_cache.TryGetValue(TeacherCacheKey, out ICollection<Teacher> cachedTeachers))
                return cachedTeachers;

            var teachersFromDb = await _context.Teachers
                .Include(t => t.Courses)
                .OrderBy(t => t.Id)
                .ToListAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(TeacherCacheKey, teachersFromDb, cacheEntryOptions);
            return teachersFromDb;
        }

        public async Task<Teacher> GetAsync(int id)
        {
            if (_cache.TryGetValue(TeacherCacheKey, out ICollection<Teacher> cachedTeachers))
            {
                var teacher = cachedTeachers.FirstOrDefault(t => t.Id == id);
                if (teacher != null)
                    return teacher;
            }

            return await _context.Teachers
                .Include(t => t.Courses)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Teachers.AnyAsync(t => t.Id == id);
        }

        public async Task<bool> CreateAsync(Teacher teacher)
        {
            await _context.Teachers.AddAsync(teacher);
            return await Save();
        }

        public async Task<bool> UpdateAsync(Teacher teacher)
        {
            _context.Teachers.Update(teacher);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var teacher = await GetAsync(id);
            if (teacher == null)
                return false;

            _context.Teachers.Remove(teacher);
            return await Save();
        }
    }
}
