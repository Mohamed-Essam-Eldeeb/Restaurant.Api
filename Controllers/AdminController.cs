using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Api.Data;
using Restaurant.Api.DTOs;
using Restaurant.Api.Models;
using BCrypt.Net;

namespace Restaurant.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly RestaurantContext _context;

        public AdminController(RestaurantContext context)
        {
            _context = context;
        }

        // ----------------- CREATE ADMIN -----------------
        [HttpPost("create")]
        public async Task<IActionResult> CreateAdmin([FromBody] UserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new { success = false, message = "Email already in use." });

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

            return Ok(new
            {
                success = true,
                message = "Admin account created successfully!",
                id = adminUser.Id
            });
        }

        // ----------------- GET ALL ADMINS -----------------
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _context.Users
                .Where(u => u.Role == "Admin")
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    u.PhoneNumber,
                    u.Address,
                    u.CreatedAt
                })
                .ToListAsync();

            return Ok(new { success = true, admins });
        }
    }
}
