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

                item.RuleFor(x => x.Notes)
                    .MaximumLength(5000).When(x => !string.IsNullOrWhiteSpace(x.Notes))
                    .WithMessage("Notes cannot exceed 5000 characters");

                item.RuleFor(x => x.AssignmentNotes)
                    .MaximumLength(2000).When(x => !string.IsNullOrWhiteSpace(x.AssignmentNotes))
                    .WithMessage("Assignment notes cannot exceed 2000 characters");

                item.RuleFor(x => x.SupplierId)
                    .GreaterThan(0).When(x => x.SupplierId.HasValue)
                    .WithMessage("Supplier ID must be greater than 0 when provided");

                item.RuleFor(x => x.ManufacturerId)
                    .GreaterThan(0).When(x => x.ManufacturerId.HasValue)
                    .WithMessage("Manufacturer ID must be greater than 0 when provided");

                item.RuleFor(x => x.PrimaryPurposId)
                    .GreaterThan(0).When(x => x.PrimaryPurposId.HasValue)
                    .WithMessage("Primary purpose ID must be greater than 0 when provided");

                // Use AssignToEmployeeId as the rule target so PropertyName is Items[i].AssignToEmployeeId (see MergeFluentValidationIntoImportPreview).
                item.RuleFor(x => x.AssignToEmployeeId)
                    .Custom((_, context) =>
                    {
                        var row = (BatchAssetUpdateItem)context.InstanceToValidate;
                        if (row.UpdateAssignment && row.AssignToEmployeeId.HasValue && row.AssignToDepartmentId.HasValue)
                        {
                            context.AddFailure("Specify either assign to employee or assign to department, not both.");
                        }
                    })
                    .When(x => x.UpdateAssignment);
            });
        }
    }
}
