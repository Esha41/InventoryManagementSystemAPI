using FluentValidation;
using Ettad.RequestManagement.Service.SupplyManagement.Dtos;

namespace Ettad.RequestManagement.Service.SupplyManagement.Validators
{
    public class UpdateSupplyDetailDtoValidator : AbstractValidator<UpdateSupplyDetailDto>
    {
        public UpdateSupplyDetailDtoValidator()
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

