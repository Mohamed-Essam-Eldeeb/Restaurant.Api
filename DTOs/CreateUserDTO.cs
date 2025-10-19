// File: CreateUserDTO.cs
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Api.DTOs
{
    public class CreateUserDTO
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        [Phone, MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }
    }
}
