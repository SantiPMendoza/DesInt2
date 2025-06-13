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

            if (!context.Students.Any() && !context.Courses.Any() && !context.Teachers.Any())
            {
                // Crear estudiantes
                var student1 = new Student { Name = "Juan Perez" };
                var student2 = new Student { Name = "Ana Gómez" };

                // Crear profesores
                var teacher1 = new Teacher { Name = "Profesor Martínez" };
                var teacher2 = new Teacher { Name = "Profesora Díaz" };

                // Crear cursos
                var course1 = new Course { Title = "Matemáticas" };
                var course2 = new Course { Title = "Historia" };

                // Relacionar estudiantes y cursos
                student1.Courses.Add(course1);
                student2.Courses.Add(course1);
                student2.Courses.Add(course2);

                // Relacionar profesores y cursos
                teacher1.Courses.Add(course1);
                teacher2.Courses.Add(course2);

                // Agregar a contexto
                await context.Students.AddRangeAsync(student1, student2);
                await context.Teachers.AddRangeAsync(teacher1, teacher2);
                await context.Courses.AddRangeAsync(course1, course2);

                await context.SaveChangesAsync();
            }
        }
    }
}
