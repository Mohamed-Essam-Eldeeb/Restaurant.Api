using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Api.Data;
using Restaurant.Api.DTOs;
using Restaurant.Api.Helpers;
using Restaurant.Api.Models;

namespace Restaurant.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] // All actions are admin-only
    public class AdminController : ControllerBase
    {
        private readonly RestaurantContext _context;

        public AdminController(RestaurantContext context)
        {
            _context = context;
        }

        // ----------------- CREATE ADMIN -----------------
        [HttpPost("create")]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateUserDTO dto)
        {
            // Email uniqueness check
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new ApiResponse<string>("Email already in use.", false));

            var adminUser = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Admin",
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address
            };

            _context.Users.Add(adminUser);
            await _context.SaveChangesAsync();

            // Return only ID to avoid exposing unnecessary data
            return Ok(new ApiResponse<int>(adminUser.Id, "Admin account created successfully!"));
        }

        // ----------------- GET ALL ADMINS -----------------
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _context.Users
                .Where(u => u.Role == "Admin")
                .Select(u => new AdminDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Address = u.Address,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();

            return Ok(new ApiResponse<IEnumerable<AdminDTO>>(admins, "List of all admins."));
        }
    }

    // ----------------- Admin Output DTO -----------------
    public class AdminDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
