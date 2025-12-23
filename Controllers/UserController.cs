using Clinical_project.Models;
using Clinical_project.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Clinical_project.Models.ViewModels.Auth;
using PatientApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clinical_project.Controllers.Auth
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokenService;
        private readonly AuthService _authService;


        public UsersController(
            UserManager<User> userManager,
            IConfiguration configuration,
            TokenService tokenService,
            AuthService authService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _authService = authService;
        }

        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Email and password are required");

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null) return BadRequest("User already exists");

            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName ?? "",
                Gender = request.Gender ?? "",
                Address = request.Address ?? "",
                PhoneNumber = request.PhoneNumber ?? "",
                Role = UserRole.Patient 
            };

            var result = await _userManager.CreateAsync(user, request.Password);


            if (result.Succeeded)
            {
                
                await _userManager.AddToRoleAsync(user, "Patient");
                return Ok(new { Message = "User created successfully as a Patient." });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Email and password are required");

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return BadRequest("Invalid credentials");

            
            var jwtToken = await _tokenService.GenerateJwtToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            await _authService.SaveRefreshTokenAsync(user, refreshToken, DateTime.UtcNow.AddDays(7));

           
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                Token = jwtToken,
                RefreshToken = refreshToken,
                Role = roles.FirstOrDefault() 
            });
        }

        
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetUserProfile()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized("Email claim missing.");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound();

            return Ok(new
            {
                user.Id,
                user.Email,
                user.FullName,
                user.Gender,
                user.BirthDate
            });
        }

        
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Ok("If user exists, password reset request has been processed.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            return Ok(new { Token = token, Message = "Password reset request processed." });
        }

        
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return BadRequest("User not found.");

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok("Password successfully reset.");
        }

        [HttpPut("change-password")]
        [Authorize] 
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email)) return Unauthorized();

            
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound("User not found.");

            
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Password changed successfully." });
            }

            return BadRequest(result.Errors);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            
            var users = await _userManager.Users
                .Select(u => new
                {
                    u.Id,
                    u.FullName,
                    u.Email,
                    u.Role, 
                    u.CreatedAt
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpPut("profile")]
        [Authorize] 
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDto request)
        {
            
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(email!);

            if (user == null) return NotFound("User not found.");

            
            user.FullName = request.FullName ?? user.FullName;
            user.Gender = request.Gender ?? user.Gender;
            user.Address = request.Address ?? user.Address;
            user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Profile updated successfully." });
            }

            return BadRequest(result.Errors);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound("User not found");

            return Ok(user);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await _authService.RefreshToken(request);
            if (!result.Success) return Unauthorized("Invalid Refresh Token");

            return Ok(new { Token = result.JwtToken, RefreshToken = result.RefreshToken });
        }
    }
}