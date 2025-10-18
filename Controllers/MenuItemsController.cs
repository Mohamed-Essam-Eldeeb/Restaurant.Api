using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Api.DTOs;
using Restaurant.Api.Services;

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

        // ----------------- GET ALL -----------------
        // Public endpoint: anyone can view menu items
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(new { success = true, items });
        }

        // ----------------- GET BY ID -----------------
        // Public endpoint
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound(new { success = false, message = "Menu item not found." });

            return Ok(new { success = true, item });
        }

        // ----------------- CREATE -----------------
        // Admin only
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] MenuItemDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid data.", errors = ModelState });

            var createdItem = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdItem.Id }, new { success = true, createdItem });
        }

        // ----------------- UPDATE -----------------
        // Admin only
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] MenuItemDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid data.", errors = ModelState });

            var updated = await _service.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new { success = false, message = "Menu item not found." });

            return Ok(new { success = true, message = "Menu item updated successfully." });
        }

        // ----------------- DELETE -----------------
        // Admin only
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { success = false, message = "Menu item not found." });

            return Ok(new { success = true, message = "Menu item deleted successfully." });
        }
    }
}
