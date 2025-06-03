using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using AtecaAPI.Data;
using AtecaAPI.Models.Entity;
using AtecaAPI.Repository.IRepository;

namespace AtecaAPI.Repository
{
    public class ProfesorRepository : IProfesorRepository
    {
        // Variables de Profesor
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        // Cambiado nombre para evitar confusión
        private readonly string ProfesorCacheKeyName = "ProfesorCacheKey";
        private readonly int CacheExpirationTime = 3600;

        /// <summary>
        /// Constructor de ProfesorRepository.
        /// </summary>
        public ProfesorRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        /// <summary>
        /// Guarda cambios en la base de datos y limpia caché si es exitoso.
        /// </summary>
        public async Task<bool> Save()
        {
            var result = await _context.SaveChangesAsync() >= 0;
            if (result)
            {
                ClearCache();
            }
            return result;
        }

        /// <summary>
        /// Limpia la caché de profesores.
        /// </summary>
        public void ClearCache()
        {
            _cache.Remove(ProfesorCacheKeyName);
        }

        /// <summary>
        /// Obtiene todos los profesores, utilizando caché para mejorar rendimiento.
        /// </summary>
        public async Task<ICollection<Profesor>> GetAllAsync()
        {
            if (_cache.TryGetValue(ProfesorCacheKeyName, out ICollection<Profesor> ProfesorCached))
                return ProfesorCached;

            var ProfesoresFromDb = await _context.Profesores.OrderBy(c => c.Id).ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(ProfesorCacheKeyName, ProfesoresFromDb, cacheEntryOptions);
            return ProfesoresFromDb;
        }

        /// <summary>
        /// Obtiene un profesor por su Id, buscando primero en caché.
        /// </summary>
        public async Task<Profesor> GetAsync(int id)
        {
            if (_cache.TryGetValue(ProfesorCacheKeyName, out ICollection<Profesor> ProfesorCached))
            {
                var Profesor = ProfesorCached.FirstOrDefault(c => c.Id == id);
                if (Profesor != null)
                    return Profesor;
            }

            return await _context.Profesores.FirstOrDefaultAsync(c => c.Id == id);
        }

        /// <summary>
        /// Obtiene un profesor por su GoogleId.
        /// </summary>
        public async Task<Profesor?> GetByGoogleIdAsync(string googleId)
        {
            return await _context.Profesores.FirstOrDefaultAsync(p => p.GoogleId == googleId);
        }

        /// <summary>
        /// Crea un nuevo profesor solo si no existe otro con el mismo GoogleId o Email.
        /// </summary>
        public async Task<bool> CreateIfNotExistsAsync(Profesor profesor)
        {
            var exists = await _context.Profesores.AnyAsync(p => p.GoogleId == profesor.GoogleId || p.Email == profesor.Email);
            if (exists) return false;

            await _context.Profesores.AddAsync(profesor);
            return await Save();
        }

        /// <summary>
        /// Verifica si existe un profesor por Id.
        /// </summary>
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Profesores.AnyAsync(c => c.Id == id);
        }

        /// <summary>
        /// Crea un nuevo profesor.
        /// </summary>
        public async Task<bool> CreateAsync(Profesor profesor)
        {
            await _context.Profesores.AddAsync(profesor);
            return await Save();
        }

        /// <summary>
        /// Actualiza un profesor existente.
        /// </summary>
        public async Task<bool> UpdateAsync(Profesor profesor)
        {
            _context.Update(profesor);
            return await Save();
        }

        /// <summary>
        /// Elimina un profesor por su Id.
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            var profesor = await GetAsync(id);
            if (profesor == null)
                return false;

            _context.Profesores.Remove(profesor);
            return await Save();
        }
    }
}
