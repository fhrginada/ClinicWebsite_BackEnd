namespace Clinical_project.Models.ViewModels.Auth;
public class UserRegisterDto
{
    public string? FullName { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

