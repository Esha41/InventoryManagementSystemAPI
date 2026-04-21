using FluentValidation;
using Ettad.RequestManagement.Service.Returns.Dtos;
using Ettad.Data.Enums;

namespace Ettad.RequestManagement.Service.Returns.Validators
{
    public class CreateReturnDtoValidator : AbstractValidator<CreateReturnDto>
    {
        public CreateReturnDtoValidator()
        {
            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Priority must be a valid value (High, Medium, Low)");

            RuleFor(x => x.RequestPurposeId)
                .GreaterThan(0).WithMessage("Request purpose is required");

            RuleFor(x => x.Reason)
                .MaximumLength(1000).WithMessage("Reason cannot exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.Reason));

            RuleFor(x => x.Notes)
                .MaximumLength(2000).WithMessage("Notes cannot exceed 2000 characters")
                .When(x => !string.IsNullOrEmpty(x.Notes));

            RuleFor(x => x.RequestPurposeNotes)
                .NotEmpty().WithMessage("Request purpose notes is required")
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Request purpose notes is required");

            RuleFor(x => x.ReturnItems)
                .NotEmpty().WithMessage("At least one return item is required")
                .Must(items => items != null && items.Count > 0)
                .WithMessage("At least one return item is required");

            RuleForEach(x => x.ReturnItems)
                .SetValidator(new CreateReturnItemDtoValidator());
        }
    }

    public class CreateReturnItemDtoValidator : AbstractValidator<CreateReturnItemDto>
    {
        public CreateReturnItemDtoValidator()
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

