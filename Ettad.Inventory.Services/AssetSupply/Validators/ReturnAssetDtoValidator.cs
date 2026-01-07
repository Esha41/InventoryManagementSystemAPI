using FluentValidation;
using Ettad.Inventory.Service.AssetSupply.Dtos;

namespace Ettad.Inventory.Service.AssetSupply.Validators
{
    public class ReturnAssetDtoValidator : AbstractValidator<ReturnAssetDto>
    {
        public ReturnAssetDtoValidator()
        {
            RuleFor(x => x.AssetId)
                .GreaterThan(0).WithMessage("Asset ID is required and must be greater than 0");

            RuleFor(x => x.ConditionOnReturn)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.ConditionOnReturn))
                .WithMessage("Condition on return cannot exceed 500 characters");

            RuleFor(x => x.Notes)
                .MaximumLength(2000).When(x => !string.IsNullOrWhiteSpace(x.Notes))
                .WithMessage("Notes cannot exceed 2000 characters");
        }
    }

    public class ReturnMultipleAssetsDtoValidator : AbstractValidator<ReturnMultipleAssetsDto>
    {
        public ReturnMultipleAssetsDtoValidator()
        {
            RuleFor(x => x.Assets)
                .NotEmpty().WithMessage("At least one asset must be returned");

            RuleFor(x => x.CommonNotes)
                .MaximumLength(2000).When(x => !string.IsNullOrWhiteSpace(x.CommonNotes))
                .WithMessage("Common notes cannot exceed 2000 characters");

            RuleForEach(x => x.Assets).SetValidator(new ReturnAssetDtoValidator());
        }
    }
}

