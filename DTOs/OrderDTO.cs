using System.ComponentModel.DataAnnotations;

namespace Restaurant.Api.DTOs
{
    public class OrderDTO
    {
        public int? UserId { get; set; } // null if guest order

        [Required(ErrorMessage = "Customer name is required.")]
        [MaxLength(100, ErrorMessage = "Customer name cannot exceed 100 characters.")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Delivery address is required.")]
        [MaxLength(255, ErrorMessage = "Delivery address cannot exceed 255 characters.")]
        public string DeliveryAddress { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Order must contain at least one item.")]
        [MinLength(1, ErrorMessage = "Order must contain at least one item.")]
        public List<OrderItemDTO> Items { get; set; } = new();

        // Optional: the status is set by the system, not the user
        public string Status { get; set; } = "Pending";
    }
}
