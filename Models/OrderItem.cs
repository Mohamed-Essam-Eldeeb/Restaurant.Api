using Restaurant.Api.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Api.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int MenuItemId { get; set; }

        public MenuItem MenuItem { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }
    }
}
