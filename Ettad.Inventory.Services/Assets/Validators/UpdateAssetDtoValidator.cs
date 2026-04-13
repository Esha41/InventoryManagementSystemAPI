using FluentValidation;
using Ettad.Inventory.Service.Assets.Dtos;

namespace Ettad.Inventory.Service.Assets.Validators
{
    public class UpdateAssetDtoValidator : AbstractValidator<UpdateAssetDto>
    {
        public UpdateAssetDtoValidator()
        {
            RuleFor(x => x.ItemId)
                .GreaterThan(0).WithMessage("Item ID is required and must be greater than 0");

            RuleFor(x => x.SerialNumber)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.SerialNumber))
                .WithMessage("Serial number cannot exceed 500 characters");

            RuleFor(x => x.RFID)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.RFID))
                .WithMessage("RFID cannot exceed 500 characters");

            RuleFor(x => x.Notes)
                .MaximumLength(5000).When(x => !string.IsNullOrWhiteSpace(x.Notes))
                .WithMessage("Notes cannot exceed 5000 characters");

            RuleFor(x => x.SupplierId)
                .GreaterThan(0).When(x => x.SupplierId.HasValue)
                .WithMessage("Supplier ID must be greater than 0 when provided");

            RuleFor(x => x.ManufacturerId)
                .GreaterThan(0).When(x => x.ManufacturerId.HasValue)
                .WithMessage("Manufacturer ID must be greater than 0 when provided");

            RuleFor(x => x.PrimaryPurposId)
                .GreaterThan(0).When(x => x.PrimaryPurposId.HasValue)
                .WithMessage("Primary purpose ID must be greater than 0 when provided");
        }
    }
}

