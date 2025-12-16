using Microsoft.AspNetCore.Identity;
using Clinical_project.Models.Entities; 

namespace Clinical_project.Models
{
    public class User : IdentityUser
    {
        public string? Status { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryDate { get; set; }

        public Patient? Patient { get; set; }
      

    }
}
