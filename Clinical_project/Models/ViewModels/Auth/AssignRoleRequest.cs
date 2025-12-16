namespace Clinical_project.Models.ViewModels.Auth
{
    public class AssignRoleRequest
    {
        public string UserId { get; set; } = null!;
        public string RoleName { get; set; } = null!;
    }
}