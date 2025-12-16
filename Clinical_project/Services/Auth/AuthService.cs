using Clinical_project.Models;
using Clinical_project.Models.ViewModels.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Clinical_project.Services.Auth
{
    public class AuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;

        public AuthService(UserManager<User> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task SaveRefreshTokenAsync(User user, string refreshToken, DateTime expiryDate)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryDate = expiryDate;
            await _userManager.UpdateAsync(user);
        }

       
        public async Task<(string JwtToken, string RefreshToken, bool Success)> RefreshToken(RefreshTokenRequest request)
        {
            
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);

            if (user == null || user.RefreshTokenExpiryDate <= DateTime.UtcNow)
            {
                return (string.Empty, string.Empty, false);
            }

           
            if (user.RefreshToken != request.RefreshToken)
            {
                return (string.Empty, string.Empty, false);
            }

            var newJwtToken = await _tokenService.GenerateJwtToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            
            await SaveRefreshTokenAsync(user, newRefreshToken, DateTime.UtcNow.AddDays(7));

            return (newJwtToken, newRefreshToken, true);
        }
    }
}