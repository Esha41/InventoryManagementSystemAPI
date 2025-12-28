using FluentValidation;
using Ettad.Inventory.Service.Assets.Dtos;

namespace Ettad.Inventory.Service.Assets.Validators
{
    public class CreateAssetDtoValidator : AbstractValidator<CreateAssetDto>
    {
        public CreateAssetDtoValidator()
        {
            RuleFor(x => x.ItemId)
                .GreaterThan(0).WithMessage("Item ID is required and must be greater than 0");

            RuleFor(x => x.DepotId)
                .GreaterThan(0).WithMessage("Depot ID is required and must be greater than 0");

            RuleFor(x => x.SerialNumber)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.SerialNumber))
                .WithMessage("Serial number cannot exceed 500 characters");

            RuleFor(x => x.RFID)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.RFID))
                .WithMessage("RFID cannot exceed 500 characters");

            RuleFor(x => x.AssetTag)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.AssetTag))
                .WithMessage("Asset tag cannot exceed 500 characters");

            RuleFor(x => x.Condition)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Condition))
                .WithMessage("Condition cannot exceed 500 characters");

            RuleFor(x => x.Notes)
                .MaximumLength(5000).When(x => !string.IsNullOrWhiteSpace(x.Notes))
                .WithMessage("Notes cannot exceed 5000 characters");
        }
    }
}

