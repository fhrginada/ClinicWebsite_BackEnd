using Microsoft.AspNetCore.Identity;
using PatientApi.Models.Entities;


namespace Clinical_project.Data.Seed
{
    public static class DefaultUsersSeeder
    {
        public static async Task SeedRolesAndUsers(
            RoleManager<IdentityRole<int>> roleManager,
            UserManager<User> userManager)
        {
            string[] roles = { "Admin", "Doctor", "Nurse" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(role));
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
                    EmailConfirmed = true,
                    Role = UserRole.Admin
                };

                await userManager.CreateAsync(user, "Admin123!");
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
