using FluentValidation;
using Ettad.Inventory.Service.Batches.Dtos;

namespace Ettad.Inventory.Service.Batches.Validators
{
    public class BulkUpdateBatchAssetsDtoValidator : AbstractValidator<BulkUpdateBatchAssetsDto>
    {
        public BulkUpdateBatchAssetsDtoValidator()
        {
            RuleFor(x => x.Items)
                .NotNull().WithMessage("Items list is required")
                .NotEmpty().WithMessage("Items list cannot be empty");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(x => x.AssetId)
                    .GreaterThan(0).WithMessage("Asset ID is required and must be greater than 0");

                item.RuleFor(x => x.ItemId)
                    .GreaterThan(0).WithMessage("Item ID is required and must be greater than 0");

                item.RuleFor(x => x.SerialNumber)
                    .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.SerialNumber))
                    .WithMessage("Serial number cannot exceed 500 characters");

                item.RuleFor(x => x.RFID)
                    .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.RFID))
                    .WithMessage("RFID cannot exceed 500 characters");

                item.RuleFor(x => x.AssetTag)
                    .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.AssetTag))
                    .WithMessage("Asset tag cannot exceed 500 characters");

                item.RuleFor(x => x.Condition)
                    .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Condition))
                    .WithMessage("Condition cannot exceed 500 characters");

                item.RuleFor(x => x.Notes)
                    .MaximumLength(5000).When(x => !string.IsNullOrWhiteSpace(x.Notes))
                    .WithMessage("Notes cannot exceed 5000 characters");
            });
        }
    }
}
