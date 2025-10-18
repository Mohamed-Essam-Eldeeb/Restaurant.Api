using System.ComponentModel.DataAnnotations;

namespace Restaurant.Api.DTOs
{
    public class OrderItemDTO
    {
        [Required(ErrorMessage = "MenuItemId is required.")]
        public int MenuItemId { get; set; }

        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100.")]
        public int Quantity { get; set; }
    }
}
