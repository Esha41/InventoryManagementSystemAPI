using FluentValidation;
using Ettad.Inventory.Service.Inventories.Dtos;

namespace Ettad.Inventory.Service.Inventories.Validators
{
    public class UpdateInventoryDtoValidator : AbstractValidator<UpdateInventoryDto>
    {
        public UpdateInventoryDtoValidator()
        {
            RuleFor(x => x.DepoId)
                .GreaterThan(0).WithMessage("Depot is required");

            RuleFor(x => x.InvoiceNumber)
                .NotEmpty().WithMessage("Invoice number is required")
                .MaximumLength(100).WithMessage("Invoice number cannot exceed 100 characters");

            RuleFor(x => x.InvoiceDate)
                .Must(date => !date.HasValue || date.Value <= DateTime.Now)
                .WithMessage("Invoice date cannot be in the future");

            RuleFor(x => x.RecievedDate)
                .Must(date => !date.HasValue || date.Value <= DateTime.Now)
                .WithMessage("Received date cannot be in the future");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters");

            RuleFor(x => x.InventoryDetails)
                .NotEmpty().WithMessage("At least one inventory detail is required")
                .Must(details => details != null && details.Count > 0)
                .WithMessage("At least one inventory detail is required");

            RuleForEach(x => x.InventoryDetails)
                .SetValidator(new UpdateInventoryDetailDtoValidator());
        }
    }

    public class UpdateInventoryDetailDtoValidator : AbstractValidator<UpdateInventoryDetailDto>
    {
        public UpdateInventoryDetailDtoValidator()
        {
            RuleFor(x => x.ItemId)
                .GreaterThan(0).WithMessage("Item is required");

            RuleFor(x => x.Lot)
                .GreaterThan(0).WithMessage("Lot number must be greater than 0");

            RuleFor(x => x.ItemQuantity)
                .GreaterThan(0).WithMessage("Item quantity must be greater than 0");

            RuleFor(x => x.CurrentQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Current quantity cannot be negative")
                .LessThanOrEqualTo(x => x.ItemQuantity).WithMessage("Current quantity cannot exceed item quantity");

            RuleFor(x => x.SupplierId)
                .GreaterThan(0).When(x => x.SupplierId.HasValue)
                .WithMessage("Supplier ID must be greater than 0 when provided");

            RuleFor(x => x.ManufacturerId)
                .GreaterThan(0).When(x => x.ManufacturerId.HasValue)
                .WithMessage("Manufacturer ID must be greater than 0 when provided");

            RuleFor(x => x.CountryId)
                .GreaterThan(0).When(x => x.CountryId.HasValue)
                .WithMessage("Country ID must be greater than 0 when provided");
        }
    }
}

