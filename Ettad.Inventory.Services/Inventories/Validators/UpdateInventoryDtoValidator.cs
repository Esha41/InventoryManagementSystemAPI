using FluentValidation;
using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.CrossCutting.Comman.Time;

namespace Ettad.Inventory.Service.Inventories.Validators
{
    public class UpdateInventoryDtoValidator : AbstractValidator<UpdateInventoryDto>
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public UpdateInventoryDtoValidator(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            RuleFor(x => x.DepoId)
                .GreaterThan(0).WithMessage("Depot is required");

            RuleFor(x => x.InvoiceNumber)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.InvoiceNumber))
                .WithMessage("Invoice number cannot exceed 100 characters");

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
                .SetValidator(new UpdateInventoryDetailDtoValidator(_dateTimeProvider));
        }
    }

    public class UpdateInventoryDetailDtoValidator : AbstractValidator<UpdateInventoryDetailDto>
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public UpdateInventoryDetailDtoValidator(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            RuleFor(x => x.ItemId)
                .GreaterThan(0).WithMessage("Item is required");

            RuleFor(x => x.Lot)
                .Must(l => !string.IsNullOrWhiteSpace(l)).WithMessage("Lot is required")
                .MaximumLength(64).WithMessage("Lot cannot exceed 64 characters");

            RuleFor(x => x.OriginalQuantity)
                .GreaterThan(0).WithMessage("Item quantity must be greater than 0");

            RuleFor(x => x.BatchNo)
                .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.BatchNo))
                .WithMessage("Batch number cannot exceed 500 characters");

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

