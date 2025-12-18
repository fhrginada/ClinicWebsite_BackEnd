using Microsoft.AspNetCore.Identity;
namespace Clinical_project.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryDate { get; set; }
    }
}