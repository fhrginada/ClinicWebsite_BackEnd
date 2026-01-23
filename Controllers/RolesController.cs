using Clinical_project.Models.ViewModels.Auth;
using Clinical_project.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PatientApi.Models.Entities;
using Microsoft.EntityFrameworkCore;



namespace Clinical_project.Controllers.Auth
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly RolesService _rolesService;

        public RolesController(RolesService rolesService)
        {
            _rolesService = rolesService;
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            var roles = _rolesService.GetAllRoles();
            return Ok(roles);
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
        {
            var result = await _rolesService.AssignRoleToUser(request.UserId, request.RoleName);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok($"Role {request.RoleName} assigned to user {request.UserId} successfully.");
        }
    }
}
