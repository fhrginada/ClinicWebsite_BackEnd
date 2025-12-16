using Clinical_project.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clinical_project.Services.Auth
{
   
    public class RolesService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public RolesService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

       
        public List<string> GetAllRoles()
        {
            var roles = new List<string>();
            foreach (var role in _roleManager.Roles)
            {
                roles.Add(role.Name!);
            }
            return roles;
        }

    
        public async Task<IdentityResult> AssignRoleToUser(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !await _roleManager.RoleExistsAsync(roleName))
            {
                return IdentityResult.Failed(new IdentityError { Description = "User or Role not found." });
            }
            return await _userManager.AddToRoleAsync(user, roleName);
        }
    }
}