using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Restaurant.Api.Data;
using Restaurant.Api.DTOs;

namespace Restaurant.Api.Validators
{
    public class CreateUserDTOValidator : AbstractValidator<CreateUserDTO>
    {
        private readonly RestaurantContext _context;

        // Inject the DbContext
        public CreateUserDTOValidator(RestaurantContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.")
                .MustAsync(async (email, cancellation) =>
                {
                    return !await _context.Users.AnyAsync(u => u.Email == email);
                })
                .WithMessage("Email already in use.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[0-9]{7,15}$")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
                .WithMessage("Invalid phone number format.")
                .MustAsync(async (phone, cancellation) =>
                {
                    return !await _context.Users.AnyAsync(u => u.PhoneNumber == phone);
                })
                .WithMessage("Phone number already in use.");

            RuleFor(x => x.Address)
                .MaximumLength(255).WithMessage("Address cannot exceed 255 characters.");
        }
    }
}
