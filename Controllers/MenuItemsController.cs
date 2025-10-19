using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Api.DTOs;
using Restaurant.Api.Helpers;
using Restaurant.Api.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemsController : ControllerBase
    {
        private readonly MenuItemService _service;

        public MenuItemsController(MenuItemService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();

            var dtos = items.Select(i => new MenuItemDTO
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                Price = i.Price,
                ImageUrl = i.ImageUrl,
                CategoryId = i.CategoryId,
                IsAvailable = i.IsAvailable
            }).ToList();

            return Ok(new ApiResponse<IEnumerable<MenuItemDTO>>(dtos));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound(new ApiResponse<string>("Menu item not found"));

            var dto = new MenuItemDTO
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                ImageUrl = item.ImageUrl,
                CategoryId = item.CategoryId,
                IsAvailable = item.IsAvailable
            };

            return Ok(new ApiResponse<MenuItemDTO>(dto));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] MenuItemDTO dto)
        {
            var createdItem = await _service.CreateAsync(dto);

            var createdDto = new MenuItemDTO
            {
                Id = createdItem.Id,
                Name = createdItem.Name,
                Description = createdItem.Description,
                Price = createdItem.Price,
                ImageUrl = createdItem.ImageUrl,
                CategoryId = createdItem.CategoryId,
                IsAvailable = createdItem.IsAvailable
            };

            return CreatedAtAction(nameof(GetById), new { id = createdDto.Id },
                new ApiResponse<MenuItemDTO>(createdDto, "Menu item created successfully"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] MenuItemDTO dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new ApiResponse<string>("Menu item not found"));

            return Ok(new ApiResponse<string>("Menu item updated successfully"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound(new ApiResponse<string>("Menu item not found"));

            return Ok(new ApiResponse<string>("Menu item deleted successfully"));
        }
    }
}
