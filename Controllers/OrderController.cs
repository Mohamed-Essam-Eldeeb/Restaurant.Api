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
            return Ok(new ApiResponse<IEnumerable<Order>>(orders));
        }

        // ----------------- GET BY ID -----------------
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _service.GetByIdAsync(id);
            if (order == null)
                return NotFound(new ApiResponse<string>("Order not found.", false));

            // Admin or owner check
            var userRole = User.FindFirst("role")?.Value;
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");

            if (userRole != "Admin" && order.UserId != userId)
                return Forbid();

            return Ok(new ApiResponse<Order>(order));
        }

        // ----------------- CREATE -----------------
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] OrderDTO dto)
        {
            // Validation happens via FluentValidation automatically

            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");

            var order = new Order
            {
                CustomerName = dto.CustomerName,
                DeliveryAddress = dto.DeliveryAddress,
                PhoneNumber = dto.PhoneNumber,
                UserId = userId,
                Items = dto.Items.Select(i => new OrderItem
                {
                    MenuItemId = i.MenuItemId,
                    Quantity = i.Quantity
                }).ToList()
            };

            var createdOrder = await _service.CreateAsync(order);

            return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id },
                new ApiResponse<Order>(createdOrder, "Order created successfully."));
        }

        // ----------------- UPDATE STATUS -----------------
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string newStatus)
        {
            if (!Enum.TryParse<OrderStatus>(newStatus, true, out var parsedStatus))
                return BadRequest(new ApiResponse<string>("Invalid status value.", false));

            var updated = await _service.UpdateStatusAsync(id, parsedStatus);
            if (!updated)
                return NotFound(new ApiResponse<string>("Order not found.", false));

            return Ok(new ApiResponse<string>("Order status updated successfully."));
        }

        // ----------------- DELETE -----------------
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound(new ApiResponse<string>("Order not found.", false));

            return Ok(new ApiResponse<string>("Order deleted successfully."));
        }
    }

    // ----------------- API Response Wrapper -----------------
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResponse(T data, string message = "")
        {
            Data = data;
            Message = message;
        }

        public ApiResponse(string message, bool success)
        {
            Success = success;
            Message = message;
        }
    }
}
