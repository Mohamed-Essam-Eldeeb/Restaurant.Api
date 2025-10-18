using System.ComponentModel.DataAnnotations;

namespace Restaurant.Api.DTOs
{
    public class OrderDTO
    {
        public int? UserId { get; set; } // null if guest order

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public List<OrderItemDTO> Items { get; set; } = new();

        public string Status { get; set; } = "Pending";
    }
}
