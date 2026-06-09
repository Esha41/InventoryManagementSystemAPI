using FluentValidation;
using Ettad.Inventory.Service.AssetSupply.Dtos;

namespace Ettad.Inventory.Service.AssetSupply.Validators
{
    public class CreateAssetSupplyDtoValidator : AbstractValidator<CreateAssetSupplyDto>
    {
        public CreateAssetSupplyDtoValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("Order ID is required and must be greater than 0");

            RuleFor(x => x.ReceiverEmployeeId)
                .GreaterThan(0).WithMessage("Receiver employee is required");

            RuleFor(x => x.SupplyDetails)
                .NotEmpty().WithMessage("At least one asset must be included in the supply");

            RuleFor(x => x.Location)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Location))
                .WithMessage("Location cannot exceed 500 characters");

            RuleFor(x => x.Notes)
                .MaximumLength(2000).When(x => !string.IsNullOrWhiteSpace(x.Notes))
                .WithMessage("Notes cannot exceed 2000 characters");

            RuleForEach(x => x.SupplyDetails).SetValidator(new CreateAssetSupplyDetailDtoValidator());
        }
    }

    public class CreateAssetSupplyDetailDtoValidator : AbstractValidator<CreateAssetSupplyDetailDto>
    {
        public CreateAssetSupplyDetailDtoValidator()
        {
            RuleFor(x => x.AssetId)
                .GreaterThan(0).WithMessage("Asset ID is required and must be greater than 0");

            RuleFor(x => x.ConditionOnSupply)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.ConditionOnSupply))
                .WithMessage("Condition on supply cannot exceed 500 characters");

            RuleFor(x => x.Notes)
                .MaximumLength(2000).When(x => !string.IsNullOrWhiteSpace(x.Notes))
                .WithMessage("Notes cannot exceed 2000 characters");

            RuleFor(x => x.Accessories)
                .Must(accessories =>
                {
                    if (accessories == null || accessories.Count == 0)
                        return true;
                    var ids = accessories.Select(a => a.AccessoryId).ToList();
                    return ids.Distinct().Count() == ids.Count;
                })
                .WithMessage("Duplicate accessory on the same asset supply line is not allowed");

            RuleForEach(x => x.Accessories).SetValidator(new CreateAssetSupplyAccessoryDtoValidator());
        }
    }

    public class CreateAssetSupplyAccessoryDtoValidator : AbstractValidator<CreateAssetSupplyAccessoryDto>
    {
        public CreateAssetSupplyAccessoryDtoValidator()
        {
            RuleFor(x => x.AccessoryId)
                .GreaterThan(0).WithMessage("Accessory ID is required and must be greater than 0");

            RuleFor(x => x.SuppliedQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Supplied quantity cannot be negative");
        }
    }
}
