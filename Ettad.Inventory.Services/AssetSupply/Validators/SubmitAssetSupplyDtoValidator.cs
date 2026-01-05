using FluentValidation;
using Ettad.Inventory.Service.AssetSupply.Dtos;

namespace Ettad.Inventory.Service.AssetSupply.Validators
{
    public class SubmitAssetSupplyDtoValidator : AbstractValidator<SubmitAssetSupplyDto>
    {
        public SubmitAssetSupplyDtoValidator()
        {
            RuleFor(x => x.SupplyDate)
                .NotEmpty().WithMessage("Supply date is required");

            RuleFor(x => x.ReceiverName)
                .MaximumLength(255).When(x => !string.IsNullOrWhiteSpace(x.ReceiverName))
                .WithMessage("Receiver name cannot exceed 255 characters");

            RuleFor(x => x.ReceiverMilitaryId)
                .MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.ReceiverMilitaryId))
                .WithMessage("Receiver military ID cannot exceed 100 characters");

            RuleFor(x => x.Notes)
                .MaximumLength(2000).When(x => !string.IsNullOrWhiteSpace(x.Notes))
                .WithMessage("Notes cannot exceed 2000 characters");
        }
    }
}

