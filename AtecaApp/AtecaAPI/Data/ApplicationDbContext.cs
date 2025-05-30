
using AtecaAPI.Models.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AtecaAPI.Data
{


    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<GrupoClase> GruposClase { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<FranjaHoraria> FranjasHorarias { get; set; }
        public DbSet<DiaNoLectivo> DiasNoLectivos { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Admin ↔ AppUser relación uno a uno
            modelBuilder.Entity<Administrador>()
                .HasOne(a => a.AppUser)
                .WithOne()
                .HasForeignKey<Administrador>(a => a.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Reserva ↔ Profesor
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
                .IsUnique();
        }
    }

}
