using FluentValidation;
using Ettad.RequestManagement.Service.Discards.Dtos;
using Ettad.Data.Enums;

namespace Ettad.RequestManagement.Service.Discards.Validators
{
    public class CreateDiscardDtoValidator : AbstractValidator<CreateDiscardDto>
    {
        public CreateDiscardDtoValidator()
        {
            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Priority must be a valid value (High, Medium, Low)");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Department is required");

            RuleFor(x => x.RequestPurposeId)
                .GreaterThan(0).WithMessage("Request purpose is required");

            RuleFor(x => x.RequesterId)
                .GreaterThan(0).When(x => x.RequesterId.HasValue)
                .WithMessage("Requester ID must be greater than 0 when provided");

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
                .SetValidator(new CreateDiscardItemDtoValidator());
        }
    }

    public class CreateDiscardItemDtoValidator : AbstractValidator<CreateDiscardItemDto>
    {
        public CreateDiscardItemDtoValidator()
        {
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

