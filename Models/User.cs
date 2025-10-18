using System.ComponentModel.DataAnnotations;

namespace Restaurant.Api.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } // store hash, not raw password

        [Required]
        public string Role { get; set; } = "Customer"; // "Admin" or "Customer"

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
