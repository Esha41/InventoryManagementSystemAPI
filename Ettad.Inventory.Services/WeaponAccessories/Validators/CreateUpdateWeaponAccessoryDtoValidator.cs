using FluentValidation;
using Ettad.Inventory.Service.WeaponAccessories.Dtos;

namespace Ettad.Inventory.Service.WeaponAccessories.Validators
{
    public class CreateUpdateWeaponAccessoryDtoValidator : AbstractValidator<CreateUpdateWeaponAccessoryDto>
    {
        public CreateUpdateWeaponAccessoryDtoValidator()
        {
            RuleFor(x => x.WeaponId)
                .GreaterThan(0).WithMessage("Weapon is required");

            RuleFor(x => x.AccessoryId)
                .GreaterThan(0).WithMessage("Accessory is required");

            RuleFor(x => x.DefaultQuantity)
                .GreaterThan(0).WithMessage("Default quantity must be greater than 0");
        }
    }
}
