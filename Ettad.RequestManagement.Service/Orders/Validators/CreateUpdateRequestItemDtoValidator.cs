using FluentValidation;
using Ettad.RequestManagement.Service.Orders.Dto;

namespace Ettad.RequestManagement.Service.Orders.Validators
{
    public class CreateUpdateRequestItemDtoValidator : AbstractValidator<CreateUpdateRequestItemDto>
    {
        public CreateUpdateRequestItemDtoValidator()
        {
            RuleFor(x => x.ItemId)
                .GreaterThan(0).WithMessage("Item ID is required and must be greater than 0");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters");
        }
    }
}

