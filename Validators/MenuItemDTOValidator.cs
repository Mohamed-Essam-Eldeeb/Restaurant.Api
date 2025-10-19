using FluentValidation;
using Restaurant.Api.DTOs;

namespace Restaurant.Api.Validators
{
    public class MenuItemDTOValidator : AbstractValidator<MenuItemDTO>
    {
        public MenuItemDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Menu item name is required.")
                .MaximumLength(100).WithMessage("Menu item name cannot exceed 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be greater than 0.");
        }
    }
}
