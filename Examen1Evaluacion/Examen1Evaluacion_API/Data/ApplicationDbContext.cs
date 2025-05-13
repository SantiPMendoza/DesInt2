namespace Examen1Evaluacion_API.Data
{
    using Examen1Evaluacion_API.Models.Entity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Planet> Planets { get; set; }
        
    }
}
