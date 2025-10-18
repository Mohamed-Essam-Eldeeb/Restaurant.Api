using System.ComponentModel.DataAnnotations;

namespace Restaurant.Api.DTOs
{
    public class MenuItemDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Menu item name is required.")]
        [MaxLength(100, ErrorMessage = "Menu item name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [MaxLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10000.")]
        public decimal Price { get; set; }

        [Url(ErrorMessage = "ImageUrl must be a valid URL.")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }  // Link to category

        public bool IsAvailable { get; set; } = true;
    }
}
