using FluentValidation;
using Ettad.RequestManagement.Service.SupplyManagement.Dtos;

namespace Ettad.RequestManagement.Service.SupplyManagement.Validators
{
    public class CreateSupplyDtoValidator : AbstractValidator<CreateSupplyDto>
    {
        public CreateSupplyDtoValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("Order ID is required");

            RuleFor(x => x.SupplyDetails)
                .NotEmpty().WithMessage("At least one supply detail is required")
                .Must(details => details != null && details.Count > 0)
                .WithMessage("At least one supply detail is required");

            RuleForEach(x => x.SupplyDetails)
                .SetValidator(new CreateSupplyDetailDtoValidator());
        }
    }

    public class CreateSupplyDetailDtoValidator : AbstractValidator<CreateSupplyDetailDto>
    {
        public CreateSupplyDetailDtoValidator()
        {
            RuleFor(x => x.ItemId)
                .GreaterThan(0).WithMessage("Item ID is required");

            RuleFor(x => x.Lot)
                .Must(l => !string.IsNullOrWhiteSpace(l)).WithMessage("Lot is required")
                .MaximumLength(64).WithMessage("Lot cannot exceed 64 characters");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");
        }
    }
}

