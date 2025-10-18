using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Api.Data;
using Restaurant.Api.DTOs;
using Restaurant.Api.Helpers;
using Restaurant.Api.Models;
using BCrypt.Net; // BCrypt.Net-Next
using System.ComponentModel.DataAnnotations; // <-- Add this


namespace Restaurant.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly RestaurantContext _context;
        private readonly JwtHelper _jwt;

        public AuthController(RestaurantContext context, JwtHelper jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        // ----------------- REGISTER -----------------
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid data.", errors = ModelState });

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new { success = false, message = "Email already in use." });

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Customer", // default role
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Registration successful!" });
        }

        // ----------------- LOGIN -----------------
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid data.", errors = ModelState });

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized(new { success = false, message = "Invalid email or password." });

            var token = _jwt.GenerateToken(user);

            return Ok(new
            {
                success = true,
                message = "Login successful!",
                token,
                user = new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Role
                }
            });
        }
    }

    // ----------------- LOGIN DTO -----------------
    public class LoginDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }
    }
}
