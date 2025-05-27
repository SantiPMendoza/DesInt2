
    using AtecaAPI.Models.Entity;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    namespace AtecaAPI.Data
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

                // 🔹 Crea rol "Administrador" si no existe
                var adminRole = "Administrador";
                if (!await roleManager.RoleExistsAsync(adminRole))
                    await roleManager.CreateAsync(new IdentityRole(adminRole));

                // 🔹 Crea usuario administrador
                var email = "admin@ateca.com";
                var adminUser = await userManager.FindByEmailAsync(email);
                if (adminUser == null)
                {
                    adminUser = new AppUser
                    {
                        UserName = email,
                        Email = email,
                        Name = "Administrador"
                    };
                    await userManager.CreateAsync(adminUser, "Abcd123!");
                    await userManager.AddToRoleAsync(adminUser, adminRole);

                    context.Administradores.Add(new Administrador
                    {
                        Nombre = "Administrador",
                        AppUserId = adminUser.Id
                    });
                }
            // 🔹 Crea usuario profesor de ejemplo
            if (!context.Profesores.Any())
            {
                context.Profesores.Add(new Profesor
                {
                    Nombre = "Profesor Ejemplo",
                    Email= email,
                    GoogleId = adminUser.Id // Asigna el mismo usuario administrador como profesor

                });
            }
            // 🔹 Crea horario base si no existe
            if (!context.DisponibilidadesAula.Any())
                {
                    var bloques = new List<DisponibilidadAula>
                {
                    new() { DiaSemana = DayOfWeek.Monday, HoraInicio = new(16, 00), HoraFin = new(16, 55) },
                    new() { DiaSemana = DayOfWeek.Monday, HoraInicio = new(17, 00), HoraFin = new(17, 55) },
                    new() { DiaSemana = DayOfWeek.Monday, HoraInicio = new(18, 00), HoraFin = new(18, 55) },
                    // ... añade más días y bloques según tu horario
                };

                    context.DisponibilidadesAula.AddRange(bloques);
                }

                // 🔹 Grupo de ejemplo
                if (!context.GruposClase.Any())
                {
                    context.GruposClase.AddRange(
                        new GrupoClase { Nombre = "2º Bachillerato" },
                        new GrupoClase { Nombre = "MULWEB3" }
                    );
                }

                // 🔹 Día no lectivo de prueba
                if (!context.DiasNoLectivos.Any())
                {
                    context.DiasNoLectivos.Add(new DiaNoLectivo
                    {
                        Fecha = DateOnly.FromDateTime(DateTime.Today.AddDays(1))
                    });
                }

                await context.SaveChangesAsync();
            }
        }
    }

