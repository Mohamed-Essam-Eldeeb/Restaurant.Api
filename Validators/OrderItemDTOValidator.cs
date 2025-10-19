using FluentValidation;
using Restaurant.Api.DTOs;

namespace Restaurant.Api.Validators
{
    public class OrderItemDTOValidator : AbstractValidator<OrderItemDTO>
    {
        public OrderItemDTOValidator()
        {
            RuleFor(x => x.MenuItemId)
                .GreaterThan(0).WithMessage("MenuItemId must be greater than 0.");

            RuleFor(x => x.Quantity)
                .InclusiveBetween(1, 100).WithMessage("Quantity must be between 1 and 100.");
        }
    }
}
