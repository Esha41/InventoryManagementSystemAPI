using FluentValidation;
using Ettad.CrossCutting.Comman.Time;
using Ettad.RequestManagement.Service.Orders.Dto;

namespace Ettad.RequestManagement.Service.Orders.Validators
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateOrderDtoValidator(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;

            RuleFor(x => x.Reason)
                .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Reason));

            RuleFor(x => x.RequestPurposeId)
                .GreaterThan(0).WithMessage("Request purpose is required");

            RuleFor(x => x.RequestPurposeNotes)
                .NotEmpty().WithMessage("Request purpose notes is required")
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Request purpose notes is required");

            // Order-Specific Properties Validation (uses local time per project convention)
            RuleFor(x => x.UsageDateFrom)
                .NotEmpty().WithMessage("Usage date from is required")
                .Must(d => GetLocalDate(d) >= _dateTimeProvider.Now.Date)
                .WithMessage("Usage date from must be today or a future date");

            RuleFor(x => x.UsageTimeFrom)
                .Must(time => time >= TimeOnly.MinValue && time <= TimeOnly.MaxValue)
                .WithMessage("Usage time from is required and must be a valid time");

            RuleFor(x => x.UsageDateTo)
                .NotEmpty().WithMessage("Usage date to is required")
                .Must(d => GetLocalDate(d) >= _dateTimeProvider.Now.Date)
                .WithMessage("Usage date to must be today or a future date")
                .GreaterThanOrEqualTo(x => x.UsageDateFrom)
                .WithMessage("Usage date to must be greater than or equal to usage date from");

            RuleFor(x => x.UsageTimeTo)
                .Must(time => time >= TimeOnly.MinValue && time <= TimeOnly.MaxValue)
                .WithMessage("Usage time to is required and must be a valid time");

            RuleFor(x => x.UsagePurpose)
                .NotEmpty().WithMessage("Usage purpose is required")
                .MaximumLength(500).WithMessage("Usage purpose cannot exceed 500 characters");

            RuleFor(x => x.AnnualDiscard)
                .GreaterThan(0).When(x => x.AnnualDiscard.HasValue)
                .WithMessage("Annual discard must be greater than 0 when provided");

            RuleFor(x => x.UsageLocation)
                .NotEmpty().WithMessage("Usage location is required")
                .MaximumLength(200).WithMessage("Usage location cannot exceed 200 characters");

            RuleFor(x => x.NumberOfOfficer)
                .GreaterThanOrEqualTo(0).When(x => x.NumberOfOfficer.HasValue)
                .WithMessage("Number of officers must be greater than or equal to 0 when provided");

            RuleFor(x => x.NumberOfOtherRank)
                .GreaterThanOrEqualTo(0).When(x => x.NumberOfOtherRank.HasValue)
                .WithMessage("Number of other ranks must be greater than or equal to 0 when provided");

            // Request Items Validation
            RuleFor(x => x.RequestItems)
                .NotEmpty().WithMessage("At least one request item is required");

            RuleForEach(x => x.RequestItems)
                .SetValidator(new CreateUpdateRequestItemDtoValidator());
        }

        /// <summary>
        /// Gets the date part in local time. API receives ISO (often UTC); convert to local for comparison.
        /// </summary>
        private static DateTime GetLocalDate(DateTime d)
        {
            return d.Kind == DateTimeKind.Utc ? d.ToLocalTime().Date : d.Date;
        }
    }
}

