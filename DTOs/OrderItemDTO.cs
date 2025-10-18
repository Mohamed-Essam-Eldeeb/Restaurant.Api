using System.ComponentModel.DataAnnotations;

namespace Restaurant.Api.DTOs
{
    public class OrderItemDTO
    {
        [Required]
        public int MenuItemId { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }
    }
}
