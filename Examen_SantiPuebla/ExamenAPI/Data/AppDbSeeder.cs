using ExamenAPI.Models.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExamenAPI.Data
{
    public static class AppDbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.MigrateAsync();

            const string adminRole = "admin";
            const string adminEmail = "admin@ateca.com";
            const string adminPassword = "Abcd123!";

            // 1. Crear Rol Administrador
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            // 2. Crear usuario administrador
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    Name = "Administrador"
                };
                await userManager.CreateAsync(adminUser, adminPassword);
                await userManager.AddToRoleAsync(adminUser, adminRole);

                context.Administradores.Add(new Administrador
                {
                    Nombre = "Administrador",
                    AppUserId = adminUser.Id
                });
                await context.SaveChangesAsync();
            }
        }
    }
}
