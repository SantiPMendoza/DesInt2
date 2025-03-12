using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models.Entity;

namespace RestAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Usuarios { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Alumno> Alumnos { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Propuesta> Propuestas { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Curso>()
                .HasMany(c => c.Profesores)
                .WithMany(p => p.Cursos)
                .UsingEntity<Dictionary<string, object>>(
                    "CursoProfesores",
                    j => j
                        .HasOne<Profesor>()
                        .WithMany()
                        .HasForeignKey("ProfesoresId")
                        .OnDelete(DeleteBehavior.NoAction),
                    j => j
                        .HasOne<Curso>()
                        .WithMany()
                        .HasForeignKey("CursosId")
                        .OnDelete(DeleteBehavior.NoAction)
                );


            modelBuilder.Entity<Curso>()
                .HasOne(c => c.Tutor)
                .WithMany()
                .HasForeignKey(c => c.TutorId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Proyecto>()
                .HasOne(p => p.Departamento)
                .WithMany() 
                .HasForeignKey(p => p.DepartamentoId)
                .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
