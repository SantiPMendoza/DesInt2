using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Examen2Evaluacion_API.Models.Entity;

namespace Examen2Evaluacion_API.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Configuración de la relación uno a muchos entre Usuario y Pedido
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Pedidos)
                .HasForeignKey(p => p.UsuarioId);


            // Configuración de la relación muchos a muchos entre Usuario y Producto
            modelBuilder.Entity<UsuarioProducto>()
    .HasKey(up => new { up.UsuarioId, up.ProductoId });

            modelBuilder.Entity<UsuarioProducto>()
                .HasOne(up => up.Usuario)
                .WithMany(u => u.UsuarioProductos)
                .HasForeignKey(up => up.UsuarioId);

            modelBuilder.Entity<UsuarioProducto>()
                .HasOne(up => up.Producto)
                .WithMany(p => p.UsuarioProductos)
                .HasForeignKey(up => up.ProductoId);


        }


    }
}
