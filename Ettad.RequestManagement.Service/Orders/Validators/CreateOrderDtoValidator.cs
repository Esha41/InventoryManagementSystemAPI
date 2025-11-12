using FluentValidation;
using Ettad.Data.Enums;
using Ettad.RequestManagement.Service.Orders.Dto;

namespace Ettad.RequestManagement.Service.Orders.Validators
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            // BaseRequest Properties Validation
            RuleFor(x => x.OrderNo)
                .NotEmpty().WithMessage("Order number is required")
                .MaximumLength(50).WithMessage("Order number cannot exceed 50 characters");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Reason is required")
                .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters");

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Invalid priority");

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Department is required");

            RuleFor(x => x.RequesterId)
                .NotEmpty().When(x => !string.IsNullOrEmpty(x.RequesterId))
                .WithMessage("Requester ID must be valid when provided");

            RuleFor(x => x.RecieverId)
                .NotEmpty().When(x => !string.IsNullOrEmpty(x.RecieverId))
                .WithMessage("Receiver ID must be valid when provided");

            RuleFor(x => x.DepotId)
                .GreaterThan(0).When(x => x.DepotId.HasValue)
                .WithMessage("Depot ID must be greater than 0 when provided");

            RuleFor(x => x.RequestPurposeId)
                .GreaterThan(0).WithMessage("Request purpose is required");

            // Order-Specific Properties Validation
            RuleFor(x => x.UsageDate)
                .NotEmpty().WithMessage("Usage date is required");

            RuleFor(x => x.UsageTime)
                .NotEmpty().WithMessage("Usage time is required");

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
    }
}

