using Microsoft.AspNetCore.Identity;
using Clinical_project.Models;
using System.Security.Claims;

namespace Clinical_project.Data.Seed
{
    public static class DefaultUsersSeeder
    {
        public static async Task SeedRolesAndUsers(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            // 1. إنشاء الأدوار الأساسية إذا لم تكن موجودة
            string[] roles = { "Admin", "Doctor", "Nurse" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. إنشاء مستخدم مسؤول (Admin) افتراضي
            var defaultAdmin = await userManager.FindByEmailAsync("admin@clinical.com");
            if (defaultAdmin == null)
            {
                var adminUser = new User
                {
                    UserName = "admin@clinical.com",
                    Email = "admin@clinical.com",
                    FullName = "System Administrator",
                    Gender = "N/A",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(adminUser, "AdminPass123!");

                // تعيين دور المسؤول
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}