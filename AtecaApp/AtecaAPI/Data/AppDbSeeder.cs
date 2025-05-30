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

            const string adminRole = "Administrador";
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
                    UserName = adminEmail,
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

            // 3. Crear profesor si no existe
            if (!context.Profesores.Any())
            {
                context.Profesores.Add(new Profesor
                {
                    Nombre = "Profesor Ejemplo",
                    Email = adminEmail,
                    GoogleId = adminUser.Id
                });
                await context.SaveChangesAsync();
            }

            // 4. Crear franjas horarias
            if (!context.FranjasHorarias.Any())
            {
                var franjas = new List<FranjaHoraria>
                {
                    new() { HoraInicio = new(16, 00), HoraFin = new(16, 55) },
                    new() { HoraInicio = new(17, 00), HoraFin = new(17, 55) },
                    new() { HoraInicio = new(18, 00), HoraFin = new(18, 55) },
                };
                context.FranjasHorarias.AddRange(franjas);
                await context.SaveChangesAsync();
            }

            // 5. Crear grupos de clase
            if (!context.GruposClase.Any())
            {
                context.GruposClase.AddRange(
                    new GrupoClase { Nombre = "2º Bachillerato" },
                    new GrupoClase { Nombre = "MULWEB3" }
                );
                await context.SaveChangesAsync();
            }

            // 6. Crear día no lectivo
            if (!context.DiasNoLectivos.Any())
            {
                context.DiasNoLectivos.Add(new DiaNoLectivo
                {
                    Fecha = DateOnly.FromDateTime(DateTime.Today.AddDays(1))
                });
                await context.SaveChangesAsync();
            }

            // 7. Crear reservas de ejemplo
            if (!context.Reservas.Any())
            {
                var profesor = await context.Profesores.FirstOrDefaultAsync();
                var grupo = await context.GruposClase.FirstOrDefaultAsync(g => g.Nombre == "MULWEB3");
                var franjas = await context.FranjasHorarias.ToListAsync();

                if (profesor == null || grupo == null || franjas.Count == 0)
                    return; // 🚨 Seguridad: no continuar si faltan datos base

                var franja16 = franjas.FirstOrDefault(f => f.HoraInicio == new TimeOnly(16, 0));
                var franja17 = franjas.FirstOrDefault(f => f.HoraInicio == new TimeOnly(17, 0));
                var franja18 = franjas.FirstOrDefault(f => f.HoraInicio == new TimeOnly(18, 0));

                if (franja16 == null || franja17 == null || franja18 == null)
                    return; // 🚨 Seguridad: franjas mal cargadas

                var hoy = DateOnly.FromDateTime(DateTime.Today);
                var fechas = Enumerable.Range(1, 5).Select(d => hoy.AddDays(d)).ToList();

                var reservasEjemplo = new List<Reserva>
                {
                    new() { Fecha = fechas[0], FranjaHorariaId = franja16.Id, ProfesorId = profesor.Id, GrupoClaseId = grupo.Id, Estado = "Pendiente", FechaSolicitud = DateTime.UtcNow },
                    new() { Fecha = fechas[1], FranjaHorariaId = franja17.Id, ProfesorId = profesor.Id, GrupoClaseId = grupo.Id, Estado = "Aprobada", FechaSolicitud = DateTime.UtcNow.AddDays(-2), FechaResolucion = DateTime.UtcNow.AddDays(-1) },
                    new() { Fecha = fechas[2], FranjaHorariaId = franja18.Id, ProfesorId = profesor.Id, GrupoClaseId = grupo.Id, Estado = "Rechazada", FechaSolicitud = DateTime.UtcNow.AddDays(-3), FechaResolucion = DateTime.UtcNow.AddDays(-2) },
                    new() { Fecha = fechas[3], FranjaHorariaId = franja16.Id, ProfesorId = profesor.Id, GrupoClaseId = grupo.Id, Estado = "Pendiente", FechaSolicitud = DateTime.UtcNow },
                    new() { Fecha = fechas[4], FranjaHorariaId = franja17.Id, ProfesorId = profesor.Id, GrupoClaseId = grupo.Id, Estado = "Cancelada", FechaSolicitud = DateTime.UtcNow.AddDays(-5), FechaResolucion = DateTime.UtcNow.AddDays(-4) }
                };

                context.Reservas.AddRange(reservasEjemplo);
                await context.SaveChangesAsync();
            }
        }
    }
}
