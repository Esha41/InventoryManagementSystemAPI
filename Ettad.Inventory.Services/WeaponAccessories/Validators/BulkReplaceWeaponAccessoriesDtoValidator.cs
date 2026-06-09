using FluentValidation;
using Ettad.Inventory.Service.WeaponAccessories.Dtos;

namespace Ettad.Inventory.Service.WeaponAccessories.Validators
{
    public class BulkReplaceWeaponAccessoriesDtoValidator : AbstractValidator<BulkReplaceWeaponAccessoriesDto>
    {
        public BulkReplaceWeaponAccessoriesDtoValidator()
        {
            RuleFor(x => x.WeaponId)
                .GreaterThan(0).WithMessage("Weapon is required");

            RuleForEach(x => x.Accessories).ChildRules(item =>
            {
                item.RuleFor(x => x.AccessoryId)
                    .GreaterThan(0).WithMessage("Accessory is required");

                item.RuleFor(x => x.DefaultQuantity)
                    .GreaterThan(0).WithMessage("Default quantity must be greater than 0");
            });

            RuleFor(x => x.Accessories)
                .Must(accessories =>
                {
                    if (accessories == null) return true;
                    var ids = accessories.Select(a => a.AccessoryId).ToList();
                    return ids.Distinct().Count() == ids.Count;
                })
                .WithMessage("Duplicate accessory entries are not allowed");
        }
    }
}
