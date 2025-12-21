using Microsoft.AspNetCore.Identity;
using Clinical_project.Models;
using System.Security.Claims;
using PatientApi.Models.Entities;



namespace Clinical_project.Data.Seed
{
    public static class DefaultUsersSeeder
    {
        public static async Task SeedRolesAndUsers(
            RoleManager<IdentityRole<string>> roleManager,
            UserManager<User> userManager)
        {
            string[] roles = { "Admin", "Doctor", "Nurse" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<string>(role));
                }
            }

            var admin = await userManager.FindByEmailAsync("admin@clinical.com");
            if (admin == null)
            {
                var user = new User
                {
                    UserName = "admin@clinical.com",
                    Email = "admin@clinical.com",
                    FullName = "System Administrator",
                    Gender = "N/A",
                    EmailConfirmed = true,
                    Role = UserRole.Admin
                };

                await userManager.CreateAsync(user, "Admin123!");
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
