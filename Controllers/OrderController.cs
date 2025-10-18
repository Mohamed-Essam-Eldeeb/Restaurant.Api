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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _service.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _service.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = new Order
            {
                CustomerName = dto.CustomerName,
                DeliveryAddress = dto.DeliveryAddress,
                PhoneNumber = dto.PhoneNumber,
                UserId = dto.UserId,
                Items = dto.Items.Select(i => new OrderItem
                {
                    MenuItemId = i.MenuItemId,
                    Quantity = i.Quantity
                }).ToList()
            };

            var createdOrder = await _service.CreateAsync(order);
            return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string newStatus)
        {
            if (!Enum.TryParse<OrderStatus>(newStatus, true, out var parsedStatus))
                return BadRequest("Invalid status value.");

            var updated = await _service.UpdateStatusAsync(id, parsedStatus);
            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
