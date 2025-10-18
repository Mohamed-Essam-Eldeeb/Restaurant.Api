using System.ComponentModel.DataAnnotations;

namespace Restaurant.Api.DTOs
{
    public class MenuItemDTO
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }  // Link to category

        public bool IsAvailable { get; set; } = true;
    }
}
