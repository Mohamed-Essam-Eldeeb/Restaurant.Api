using Restaurant.Api.Models;

namespace Restaurant.Api.DTOs
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } = "Customer";
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        public UserResponseDTO(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            Role = user.Role;
            PhoneNumber = user.PhoneNumber;
            Address = user.Address;
        }
    }
}
