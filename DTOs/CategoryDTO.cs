using System.ComponentModel.DataAnnotations;

namespace Restaurant.Api.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
