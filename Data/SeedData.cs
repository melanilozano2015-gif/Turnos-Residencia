using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ResidenciaApp.Models;

namespace ResidenciaApp.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        string[] roles = new[] { "Director", "Educador", "Dirección" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var adminEmail = "admin@arguelloinfancias.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Director");
            }
        }

        using var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

        if (!context.NinosAdolescentes.Any())
        {
            context.NinosAdolescentes.AddRange(
                new NinoAdolescente
                {
                    Dni = "12345678",
                    Nombre = "Juan",
                    Apellido = "Pérez",
                    FechaNacimiento = new DateTime(2010, 5, 15),
                    Genero = "Masculino"
                },
                new NinoAdolescente
                {
                    Dni = "87654321",
                    Nombre = "Ana",
                    Apellido = "García",
                    FechaNacimiento = new DateTime(2012, 8, 20),
                    Genero = "Femenino"
                }
            );
            await context.SaveChangesAsync();
        }

        if (!context.Instituciones.Any())
        {
            context.Instituciones.AddRange(
                new Institucion
                {
                    Nombre = "Hospital General de Niños",
                    Calle = "San Martín",
                    Numero = "1234",
                    Telefono = "011-4567-8900",
                    Email = "contacto@hgn.gov.ar"
                },
                new Institucion
                {
                    Nombre = "Centro de Salud Mental",
                    Calle = "Belgrano",
                    Numero = "567",
                    Telefono = "011-5555-1234",
                    Email = "info@csm.gov.ar"
                }
            );
            await context.SaveChangesAsync();
        }
    }
}
