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
            base.OnModelCreating(modelBuilder);
            /** Configuración de la relación uno a muchos entre Usuario y Pedido
            
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Pedidos)
                .HasForeignKey(p => p.UsuarioId);*/


            // Relación uno a muchos entre Pedido y Producto explicita
            modelBuilder.Entity<PedidoProducto>()
    .HasKey(pp => new { pp.PedidoId, pp.ProductoId });

            modelBuilder.Entity<PedidoProducto>()
                .HasOne(pp => pp.Pedido)
                .WithMany(p => p.PedidoProductos)
                .HasForeignKey(pp => pp.PedidoId);

            modelBuilder.Entity<PedidoProducto>()
                .HasOne(pp => pp.Producto)
                .WithMany(p => p.PedidoProductos)
                .HasForeignKey(pp => pp.ProductoId);

            /**
             * Relación muchos a muchos entre Pedido y Producto (implicita)
             * 
                modelBuilder.Entity<Pedido>()
                .HasMany(p => p.Productos)
                .WithMany(pr => pr.Pedidos)
                .UsingEntity<Dictionary<string, object>>(
                 "PedidoProductos",
                    j => j.HasOne<Producto>().WithMany().HasForeignKey("ProductoId"),
                    j => j.HasOne<Pedido>().WithMany().HasForeignKey("PedidoId"),
                    j => j.HasKey("PedidoId", "ProductoId"));
             * */

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
