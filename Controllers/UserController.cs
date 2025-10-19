// File: UsersController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Api.DTOs;
using Restaurant.Api.Helpers;
using Restaurant.Api.Models;
using Restaurant.Api.Services;

namespace Restaurant.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _service;

        public UsersController(UserService service)
        {
            _service = service;
        }

        // GET: api/users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _service.GetAllAsync();
            var dtoList = users.Select(u => new UserResponseDTO(u)).ToList();
            return Ok(new ApiResponse<List<UserResponseDTO>>(dtoList));
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null)
                return NotFound(new ApiResponse<string>("User not found"));

            var dto = new UserResponseDTO(user);
            return Ok(new ApiResponse<UserResponseDTO>(dto));
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO dto)
        {
            var user = await _service.CreateUserAsync(dto);
            var response = new UserResponseDTO(user);
            return Ok(new ApiResponse<UserResponseDTO>(response));
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateUserDTO dto)
        {
            var updatedUser = await _service.UpdateUserAsync(id, dto);
            if (updatedUser == null)
                return NotFound(new ApiResponse<string>("User not found"));

            var response = new UserResponseDTO(updatedUser);
            return Ok(new ApiResponse<UserResponseDTO>(response));
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteUserAsync(id);
            if (!deleted)
                return NotFound(new ApiResponse<string>("User not found"));

            return Ok(new ApiResponse<string>("User deleted successfully"));
        }
    }
}
