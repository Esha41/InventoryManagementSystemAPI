using FluentValidation;
using Ettad.Data.Enums;
using Ettad.RequestManagement.Service.RequestPurposes.Dtos;

namespace Ettad.RequestManagement.Service.RequestPurposes.Validators
{
    public class CreateUpdateRequestPurposeDtoValidator : AbstractValidator<CreateUpdateRequestPurposeDto>
    {
        public CreateUpdateRequestPurposeDtoValidator()
        {
            RuleFor(x => x.NameAr)
                .NotEmpty().WithMessage("Arabic name is required")
                .MaximumLength(500).WithMessage("Arabic name cannot exceed 500 characters");

            RuleFor(x => x.NameEn)
                .NotEmpty().WithMessage("English name is required")
                .MaximumLength(500).WithMessage("English name cannot exceed 500 characters");

            RuleFor(x => x.AllowanceContext)
                .Must(ctx => !ctx.HasValue || Enum.IsDefined(typeof(RequestPurposeAllowanceContext), ctx.Value))
                .WithMessage("Allowance context must be FromAllowance, OutsideAllowance, or Both");

            RuleForEach(x => x.AttachmentRequirements)
                .SetValidator(new CreateUpdateAttachmentRequirementDtoValidator());
        }
    }
}

