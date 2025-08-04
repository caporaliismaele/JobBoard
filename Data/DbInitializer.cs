using JobBoard.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace JobBoard.Data
{
    public static class DbInitializer
    {
            public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<JobBoardUser>>();

                string[] roleNames = { "CANDIDATE", "RECRUITER", "ADMIN" };

                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                // Crea utente Admin
                string adminEmail = "admin@jobboard.com";
                string adminPassword = "Admin123!"; // Usa una password sicura in produzione

                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var adminUser = new JobBoardUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(adminUser, adminPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "ADMIN");
                    }
                }
        }

    }
}
