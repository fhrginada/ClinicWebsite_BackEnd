
using Microsoft.AspNetCore.Identity;
using Clinical_project.Models;
using System.Threading.Tasks;

namespace Clinical_project.Data.Seed
{
    public static class DefaultUsersSeeder
    {
        public static async Task SeedRolesAndUsers(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
           
            string[] roles = { "Admin", "Doctor", "Nurse" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

          
            var defaultAdmin = await userManager.FindByEmailAsync("admin@clinical.com");
            if (defaultAdmin == null)
            {
                var adminUser = new User
                {
                    UserName = "admin@clinical.com",
                    Email = "admin@clinical.com",
                    
                    EmailConfirmed = true
                };
               
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
