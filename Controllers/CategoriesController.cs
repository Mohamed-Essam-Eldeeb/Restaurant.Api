using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Api.DTOs;
using Restaurant.Api.Models;
using Restaurant.Api.Services;

namespace Restaurant.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _service;

        public CategoriesController(CategoryService service)
        {
            _service = service;
        }

        // ----------------- GET ALL -----------------
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _service.GetAllAsync();

            // Map entities to DTOs
            var categoriesDto = categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name
            });

            return Ok(new ApiResponse<IEnumerable<CategoryDTO>>(categoriesDto, "List of all categories."));
        }

        // ----------------- GET BY ID -----------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _service.GetByIdAsync(id);
            if (category == null)
                return NotFound(new ApiResponse<string>("Category not found.", false));

            var categoryDto = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name
            };

            return Ok(new ApiResponse<CategoryDTO>(categoryDto, "Category details."));
        }

        // ----------------- CREATE -----------------
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CategoryDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>("Invalid data.", false));

            var created = await _service.CreateAsync(dto);

            var createdDto = new CategoryDTO
            {
                Id = created.Id,
                Name = created.Name
            };

            return CreatedAtAction(nameof(GetById), new { id = createdDto.Id },
                new ApiResponse<CategoryDTO>(createdDto, "Category created successfully."));
        }

        // ----------------- UPDATE -----------------
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>("Invalid data.", false));

            var updated = await _service.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new ApiResponse<string>("Category not found.", false));

            return Ok(new ApiResponse<string>("Category updated successfully."));
        }

        // ----------------- DELETE -----------------
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound(new ApiResponse<string>("Category not found.", false));

            return Ok(new ApiResponse<string>("Category deleted successfully."));
        }
    }
}
