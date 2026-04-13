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

            RuleFor(x => x.BatchNumber)
                .NotEmpty().WithMessage("Batch number is required")
                .MaximumLength(500).WithMessage("Batch number cannot exceed 500 characters");

            RuleFor(x => x.SerialNumber)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.SerialNumber))
                .WithMessage("Serial number cannot exceed 500 characters");

            RuleFor(x => x.RFID)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.RFID))
                .WithMessage("RFID cannot exceed 500 characters");

            RuleFor(x => x.Notes)
                .MaximumLength(5000).When(x => !string.IsNullOrWhiteSpace(x.Notes))
                .WithMessage("Notes cannot exceed 5000 characters");

            RuleFor(x => x.AssignToEmployeeId)
                .GreaterThan(0).When(x => x.AssignToEmployeeId.HasValue)
                .WithMessage("Assign to employee ID must be greater than 0 when provided");

            RuleFor(x => x.AssignToDepartmentId)
                .GreaterThan(0).When(x => x.AssignToDepartmentId.HasValue)
                .WithMessage("Assign to department ID must be greater than 0 when provided");

            RuleFor(x => x.SupplierId)
                .GreaterThan(0).When(x => x.SupplierId.HasValue)
                .WithMessage("Supplier ID must be greater than 0 when provided");

            RuleFor(x => x.ManufacturerId)
                .GreaterThan(0).When(x => x.ManufacturerId.HasValue)
                .WithMessage("Manufacturer ID must be greater than 0 when provided");

            RuleFor(x => x.PrimaryPurposId)
                .GreaterThan(0).When(x => x.PrimaryPurposId.HasValue)
                .WithMessage("Primary purpose ID must be greater than 0 when provided");

            RuleFor(x => x.AssignmentNotes)
                .MaximumLength(2000).When(x => !string.IsNullOrWhiteSpace(x.AssignmentNotes))
                .WithMessage("Assignment notes cannot exceed 2000 characters");

            RuleFor(x => x)
                .Must(x => !(x.AssignToEmployeeId.HasValue && x.AssignToDepartmentId.HasValue))
                .WithMessage("Specify either assign to employee or assign to department, not both.");

            RuleFor(x => x)
                .Must(x => string.IsNullOrWhiteSpace(x.AssignmentNotes) ||
                          x.AssignToEmployeeId.HasValue || x.AssignToDepartmentId.HasValue)
                .WithMessage("Assignment notes require at least one of assign to employee or assign to department.");
        }
    }
}

