using FluentValidation;
using Ettad.Inventory.Service.Assets.Dtos;

namespace Ettad.Inventory.Service.Assets.Validators
{
    public class CreateBulkAssetsFromTemplateDtoValidator : AbstractValidator<CreateBulkAssetsFromTemplateDto>
    {
        public CreateBulkAssetsFromTemplateDtoValidator()
        {
            Include(new CreateAssetDtoValidator());

            RuleFor(x => x.Quantity)
                .InclusiveBetween(1, 1_000_000)
                .WithMessage("Quantity must be between 1 and 1,000,000.");

            RuleFor(x => x)
                .Must(x => x.Quantity <= 1
                    || (string.IsNullOrWhiteSpace(x.SerialNumber) && string.IsNullOrWhiteSpace(x.RFID)))
                .WithMessage("Serial Number and RFID must be empty when creating more than one asset.");
        }
    }
}
