using Restaurant.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Api.API.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        [InverseProperty("Order")]
        public List<OrderItem> Items { get; set; } = new();

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Required, MaxLength(100)]
        public string CustomerName { get; set; }

        [Required, MaxLength(255)]
        public string DeliveryAddress { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum OrderStatus
    {
        Pending,
        Preparing,
        OutForDelivery,
        Delivered,
        Cancelled
    }
}
