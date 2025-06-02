using ExamenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExamenApi.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            // Aplicar migraciones pendientes
            await context.Database.MigrateAsync();

            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // --- Usuarios Identity ---
            // Administrador
            string adminRole = "admin";
            string adminUserName = "admin";
            string adminEmail = "admin@example.com";
            string adminPassword = "UnaContraseñaSegura@123";

            if (!await roleManager.RoleExistsAsync(adminRole))
                await roleManager.CreateAsync(new IdentityRole(adminRole));

            var adminUser = await userManager.FindByNameAsync(adminUserName);
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    Name = "Administrador"
                };
                var resultAdmin = await userManager.CreateAsync(adminUser, adminPassword);
                if (!resultAdmin.Succeeded)
                    throw new Exception("Error al crear el usuario admin.");
                await userManager.AddToRoleAsync(adminUser, adminRole);
            }

            // --- Usuarios Identity ---
            // --- Usuario AngularInfoUser ---
            string userRole = "AngularInfoUser";
            string userUserName = "AngularInfoUser";
            string userEmail = "info@example.com";
            string userPassword = "G,CKvs4j7nR+-Tm!{";

            if (!await roleManager.RoleExistsAsync(userRole))
                await roleManager.CreateAsync(new IdentityRole(userRole));

            var infoUser = await userManager.FindByNameAsync(userUserName);
            if (infoUser == null)
            {
                infoUser = new AppUser
                {
                    UserName = userUserName,
                    Email = userEmail,
                    Name = "AngularInfoUser"
                };

                var resultUser = await userManager.CreateAsync(infoUser, userPassword);
                if (!resultUser.Succeeded)
                    throw new Exception("Error al crear el usuario AngularInfoUser.");

                await userManager.AddToRoleAsync(infoUser, userRole); // <-- Esto es lo que faltaba
            }


        }
    }
}
