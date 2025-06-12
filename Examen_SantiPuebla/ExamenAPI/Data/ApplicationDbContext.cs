
using ExamenAPI.Models.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExamenAPI.Data
{


    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        //public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
        //public DbSet<GrupoClase> GruposClase { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /**
             * Configuración de las relaciones entre entidades
             * */
            // Admin ↔ AppUser relación uno a uno
            modelBuilder.Entity<Administrador>()
                .HasOne(a => a.AppUser)
                .WithOne()
                .HasForeignKey<Administrador>(a => a.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);
            /**
            // Reserva ↔ Profesor: un profesor puede tener varias reservas
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Profesor)
                .WithMany(p => p.Reservas)
                .HasForeignKey(r => r.ProfesorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Reserva ↔ GrupoClase
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.GrupoClase)
                .WithMany(g => g.Reservas)
                .HasForeignKey(r => r.GrupoClaseId)
                .OnDelete(DeleteBehavior.Cascade);
            /**
             * Configuración de las entidades
             * - Profesor: Un profesor puede tener varias reservas.
             * - Reserva: Una reserva pertenece a un profesor y a un grupo de clase.
             * - FranjaHoraria: Define los tramos horarios disponibles para reservas.
             * - DiaNoLectivo: Define los días no lectivos.
             *
            // Unique: Un profesor no puede tener dos reservas iguales
            modelBuilder.Entity<Reserva>()
                .HasIndex(r => new { r.Fecha, r.FranjaHorariaId, r.ProfesorId })
                .IsUnique();


            // Unique: No se pueden duplicar tramos de disponibilidad
            modelBuilder.Entity<FranjaHoraria>()
                .HasIndex(d => new { d.HoraInicio, d.HoraFin })
                .IsUnique();

            // Unique: evitar duplicar días no lectivos
            modelBuilder.Entity<DiaNoLectivo>()
                .HasIndex(d => d.Fecha)
                .IsUnique();*/
        }
    }

}
