using FluentValidation;
using Ettad.RequestManagement.Service.Discards.Dtos;
using Ettad.Data.Enums;

namespace Ettad.RequestManagement.Service.Discards.Validators
{
    public class UpdateDiscardDtoValidator : AbstractValidator<UpdateDiscardDto>
    {
        public UpdateDiscardDtoValidator()
        {
            RuleFor(x => x.Priority)
                .NotEmpty().WithMessage("Priority is required")
                .Must(p => Enum.TryParse<RequestPriority>(p, true, out _))
                .WithMessage("Priority must be a valid value (High, Medium, Low)");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Department is required");

            RuleFor(x => x.RequestPurposeId)
                .GreaterThan(0).WithMessage("Request purpose is required");

            RuleFor(x => x.RequesterId)
                .GreaterThan(0).When(x => x.RequesterId.HasValue)
                .WithMessage("Requester ID must be greater than 0 when provided");

            RuleFor(x => x.RecieverId)
                .GreaterThan(0).When(x => x.RecieverId.HasValue)
                .WithMessage("Receiver ID must be greater than 0 when provided");

            RuleFor(x => x.DepotId)
                .GreaterThan(0).When(x => x.DepotId.HasValue)
                .WithMessage("Depot ID must be greater than 0 when provided");

            RuleFor(x => x.Reason)
                .MaximumLength(1000).WithMessage("Reason cannot exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.Reason));

            RuleFor(x => x.Notes)
                .MaximumLength(2000).WithMessage("Notes cannot exceed 2000 characters")
                .When(x => !string.IsNullOrEmpty(x.Notes));

            RuleFor(x => x.DiscardItems)
                .NotEmpty().WithMessage("At least one discard item is required")
                .Must(items => items != null && items.Count > 0)
                .WithMessage("At least one discard item is required");

            RuleForEach(x => x.DiscardItems)
                .SetValidator(new UpdateDiscardItemDtoValidator());
        }
    }

    public class UpdateDiscardItemDtoValidator : AbstractValidator<UpdateDiscardItemDto>
    {
        public UpdateDiscardItemDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).When(x => x.Id.HasValue)
                .WithMessage("Item ID must be greater than 0 when provided");

            RuleFor(x => x.ItemId)
                .GreaterThan(0).WithMessage("Item is required");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Notes));
        }
    }
}
