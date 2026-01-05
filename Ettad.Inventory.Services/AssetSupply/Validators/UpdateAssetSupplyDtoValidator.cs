using FluentValidation;
using Ettad.Inventory.Service.AssetSupply.Dtos;

namespace Ettad.Inventory.Service.AssetSupply.Validators
{
    public class UpdateAssetSupplyDtoValidator : AbstractValidator<UpdateAssetSupplyDto>
    {
        public UpdateAssetSupplyDtoValidator()
        {
            RuleFor(x => x.Location)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Location))
                .WithMessage("Location cannot exceed 500 characters");

            RuleFor(x => x.Notes)
                .MaximumLength(2000).When(x => !string.IsNullOrWhiteSpace(x.Notes))
                .WithMessage("Notes cannot exceed 2000 characters");

            When(x => x.SupplyDetails != null && x.SupplyDetails.Any(), () =>
            {
                RuleForEach(x => x.SupplyDetails).SetValidator(new CreateAssetSupplyDetailDtoValidator());
            });
        }
    }
}

