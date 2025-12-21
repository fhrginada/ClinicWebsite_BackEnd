using Microsoft.AspNetCore.Identity;
using PatientApi.Models.Entities;

namespace Clinical_project.Services.Auth
{
    public class RolesService
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly UserManager<User> _userManager;

        public RolesService(
            RoleManager<IdentityRole<int>> roleManager,
            UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public List<string> GetAllRoles()
        {
            return _roleManager.Roles
                .Select(r => r.Name!)
                .ToList();
        }

        public async Task<IdentityResult> AssignRoleToUser(int userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || !await _roleManager.RoleExistsAsync(roleName))
            {
                return IdentityResult.Failed(
                    new IdentityError { Description = "User or Role not found." });
            }

            return await _userManager.AddToRoleAsync(user, roleName);
        }
    }
}
