using FluentValidation;
using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.CrossCutting.Comman.Time;

namespace Ettad.Inventory.Service.Inventories.Validators
{
    public class CreateInventoryDtoValidator : AbstractValidator<CreateInventoryDto>
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateInventoryDtoValidator(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            RuleFor(x => x.DepoId)
                .GreaterThan(0).WithMessage("Depot is required");

            RuleFor(x => x.InvoiceNumber)
                .MaximumLength(255).WithMessage("Invoice number cannot exceed 255 characters")
                .When(x => !string.IsNullOrEmpty(x.InvoiceNumber));

            RuleFor(x => x.InvoiceDate)
                .Must(date => !date.HasValue || date.Value <= _dateTimeProvider.Now)
                .WithMessage("Invoice date cannot be in the future");

            RuleFor(x => x.RecievedDate)
                .Must(date => !date.HasValue || date.Value <= _dateTimeProvider.Now)
                .WithMessage("Received date cannot be in the future");

            RuleFor(x => x.InventoryDetails)
                .NotEmpty().WithMessage("At least one inventory detail is required")
                .Must(details => details != null && details.Count > 0)
                .WithMessage("At least one inventory detail is required");

            RuleForEach(x => x.InventoryDetails)
                .SetValidator(new CreateInventoryDetailDtoValidator(_dateTimeProvider));
        }
    }

    public class CreateInventoryDetailDtoValidator : AbstractValidator<CreateInventoryDetailDto>
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateInventoryDetailDtoValidator(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            RuleFor(x => x.ItemId)
                .GreaterThan(0).WithMessage("Item is required");

            RuleFor(x => x.Lot)
                .GreaterThan(0).WithMessage("Lot number must be greater than 0");

            RuleFor(x => x.OriginalQuantity)
                .GreaterThan(0).WithMessage("Item quantity must be greater than 0");

            RuleFor(x => x.BatchNo)
                .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.BatchNo))
                .WithMessage("Batch number cannot exceed 500 characters");

            RuleFor(x => x.ExpiryDate)
                .Must(date => !date.HasValue || date.Value > _dateTimeProvider.Now)
                .WithMessage("Expiry date must be in the future");

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

