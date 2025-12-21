namespace Clinical_project.Models.ViewModels.Auth
{
    public class AssignRoleRequest
    {
        public int UserId { get; set; } 
        public string RoleName { get; set; } = null!;
    }
}
