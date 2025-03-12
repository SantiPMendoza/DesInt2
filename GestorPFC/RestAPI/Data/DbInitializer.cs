using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Data
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

            // --- Datos del Dominio ---
            // Departamento
            if (!context.Departamentos.Any())
            {
                var departamento = new Departamento { Nombre = "Departamento Inicial" };
                context.Departamentos.Add(departamento);
                await context.SaveChangesAsync();
            }
            var dept = context.Departamentos.First();

            // Profesor (dominio)
            if (!context.Profesores.Any())
            {
                var profesor = new Profesor
                {
                    Nombre = "Profesor Inicial",
                    Apellido = "ApellidoInicial",
                    Email = "profesor.inicial@example.com",
                    DepartamentoId = dept.Id,
                    Departamento = dept
                };
                context.Profesores.Add(profesor);
                await context.SaveChangesAsync();
            }

            // Curso
            if (!context.Cursos.Any())
            {
                var tutor = context.Profesores.First();
                var curso = new Curso
                {
                    Nombre = "Curso Inicial",
                    DepartamentoId = dept.Id,
                    Departamento = dept,
                    TutorId = tutor.Id,
                    Tutor = tutor
                };
                context.Cursos.Add(curso);
                await context.SaveChangesAsync();
            }

            // Alumno (dominio)
            if (!context.Alumnos.Any())
            {
                var curso = context.Cursos.First();
                var alumno = new Alumno
                {
                    Nombre = "Alumno Inicial",
                    Apellidos = "ApellidoInicial",
                    Email = "alumno.inicial@example.com",
                    CursoId = curso.Id,
                    Curso = curso
                };
                context.Alumnos.Add(alumno);
                await context.SaveChangesAsync();
            }

            // --- Usuarios Identity para Alumno y Profesor ---
            // Usuario para Alumno
            string alumnoRole = "alumno";
            string alumnoUserName = "alumno";
            string alumnoUserEmail = "alumno.inicial@example.com";
            string alumnoPassword = "Alumno@123";

            if (!await roleManager.RoleExistsAsync(alumnoRole))
                await roleManager.CreateAsync(new IdentityRole(alumnoRole));

            var alumnoUser = await userManager.FindByNameAsync(alumnoUserName);
            if (alumnoUser == null)
            {
                alumnoUser = new AppUser
                {
                    UserName = alumnoUserName,
                    Email = alumnoUserEmail,
                    Name = "Alumno Inicial"
                };
                var resultAlumno = await userManager.CreateAsync(alumnoUser, alumnoPassword);
                if (!resultAlumno.Succeeded)
                    throw new Exception("Error al crear el usuario alumno.");
                await userManager.AddToRoleAsync(alumnoUser, alumnoRole);
            }

            // Usuario para Profesor
            string profesorRole = "profesor";
            string profesorUserName = "profesor";
            string profesorUserEmail = "profesor.inicial@example.com";
            string profesorPassword = "Profesor@123";

            if (!await roleManager.RoleExistsAsync(profesorRole))
                await roleManager.CreateAsync(new IdentityRole(profesorRole));

            var profesorUser = await userManager.FindByNameAsync(profesorUserName);
            if (profesorUser == null)
            {
                profesorUser = new AppUser
                {
                    UserName = profesorUserName,
                    Email = profesorUserEmail,
                    Name = "Profesor Inicial"
                };
                var resultProfesor = await userManager.CreateAsync(profesorUser, profesorPassword);
                if (!resultProfesor.Succeeded)
                    throw new Exception("Error al crear el usuario profesor.");
                await userManager.AddToRoleAsync(profesorUser, profesorRole);
            }
        }
    }
}
