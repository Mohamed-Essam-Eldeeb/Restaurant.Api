using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Api.API.Models;
using Restaurant.Api.DTOs;
using Restaurant.Api.Models;
using Restaurant.Api.Services;

namespace Restaurant.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _service;

        public OrdersController(OrderService service)
        {
            _service = service;
        }

        // ----------------- GET ALL -----------------
        // Admin only
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _service.GetAllAsync();
            return Ok(new { success = true, orders });
        }

        // ----------------- GET BY ID -----------------
        // Admin or the owner of the order
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _service.GetByIdAsync(id);
            if (order == null)
                return NotFound(new { success = false, message = "Order not found." });

            var userRole = User.FindFirst("role")?.Value;
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");

            if (userRole != "Admin" && order.UserId != userId)
                return Forbid();

            return Ok(new { success = true, order });
        }

        // ----------------- CREATE -----------------
        // Customer only (any logged-in user)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] OrderDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid data.", errors = ModelState });

            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");

            var order = new Order
            {
                CustomerName = dto.CustomerName,
                DeliveryAddress = dto.DeliveryAddress,
                PhoneNumber = dto.PhoneNumber,
                UserId = userId, // Always use the authenticated user ID
                Items = dto.Items.Select(i => new OrderItem
                {
                    MenuItemId = i.MenuItemId,
                    Quantity = i.Quantity
                }).ToList()
            };

            var createdOrder = await _service.CreateAsync(order);
            return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, new { success = true, createdOrder });
        }

        // ----------------- UPDATE STATUS -----------------
        // Admin only
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string newStatus)
        {
            if (!Enum.TryParse<OrderStatus>(newStatus, true, out var parsedStatus))
                return BadRequest(new { success = false, message = "Invalid status value." });

            var updated = await _service.UpdateStatusAsync(id, parsedStatus);
            if (!updated)
                return NotFound(new { success = false, message = "Order not found." });

            return Ok(new { success = true, message = "Order status updated successfully." });
        }

        // ----------------- DELETE -----------------
        // Admin only
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { success = false, message = "Order not found." });

            return Ok(new { success = true, message = "Order deleted successfully." });
        }
    }
}
