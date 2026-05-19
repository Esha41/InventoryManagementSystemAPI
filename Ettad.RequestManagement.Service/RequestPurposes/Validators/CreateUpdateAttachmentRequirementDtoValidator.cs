using FluentValidation;
using Ettad.RequestManagement.Service.RequestPurposes.Dtos;

namespace Ettad.RequestManagement.Service.RequestPurposes.Validators
{
    public class CreateUpdateAttachmentRequirementDtoValidator : AbstractValidator<CreateUpdateAttachmentRequirementDto>
    {
        public CreateUpdateAttachmentRequirementDtoValidator()
        {
            RuleFor(x => x.NameAr)
                .NotEmpty().WithMessage("Attachment requirement Arabic name is required")
                .MaximumLength(500).WithMessage("Attachment requirement Arabic name cannot exceed 500 characters");

            RuleFor(x => x.NameEn)
                .NotEmpty().WithMessage("Attachment requirement English name is required")
                .MaximumLength(500).WithMessage("Attachment requirement English name cannot exceed 500 characters");

            RuleFor(x => x.MinCount)
                .GreaterThanOrEqualTo(0).WithMessage("MinCount must be zero or greater");

            RuleFor(x => x.MaxCount)
                .GreaterThanOrEqualTo(1).WithMessage("MaxCount must be at least 1");

            RuleFor(x => x)
                .Must(x => x.MaxCount >= x.MinCount)
                .WithMessage("MaxCount must be greater than or equal to MinCount");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("DisplayOrder cannot be negative");
        }
    }
}
