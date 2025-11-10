using FluentValidation;
using Ettad.Inventory.Service.Ammunitions.Dtos;

namespace Ettad.Inventory.Service.Ammunitions.Validators
{
    public class CreateUpdateAmmunitionDtoValidator : AbstractValidator<CreateUpdateAmmunitionDto>
    {
        public CreateUpdateAmmunitionDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

            RuleFor(x => x.ItemNo)
                .NotEmpty().WithMessage("Item number is required")
                .MaximumLength(100).WithMessage("Item number cannot exceed 100 characters");

            RuleFor(x => x.BatchNo)
                .NotEmpty().WithMessage("Batch number is required")
                .MaximumLength(100).WithMessage("Batch number cannot exceed 100 characters");

            RuleFor(x => x.HccId)
                .GreaterThan(0).WithMessage("HCC is required");

            RuleFor(x => x.PartNo)
                .MaximumLength(100).WithMessage("Part number cannot exceed 100 characters");

            RuleFor(x => x.ExpiryDate)
                .Must(date => !date.HasValue || date.Value > DateTime.Now)
                .WithMessage("Expiry date must be in the future");

            RuleFor(x => x.BulletDiameter)
                .GreaterThan(0).WithMessage("Bullet diameter must be greater than 0");

            RuleFor(x => x.BulletDiameterUnitId)
                .GreaterThan(0).WithMessage("Bullet diameter unit is required");

            RuleFor(x => x.CaseLength)
                .GreaterThan(0).WithMessage("Case length must be greater than 0");

            RuleFor(x => x.CaseLengthUnitId)
                .GreaterThan(0).WithMessage("Case length unit is required");

            RuleFor(x => x.Primer)
                .NotEmpty().WithMessage("Primer is required")
                .MaximumLength(100).WithMessage("Primer cannot exceed 100 characters");

            RuleFor(x => x.TotalWeight)
                .GreaterThan(0).WithMessage("Total weight must be greater than 0");


            RuleFor(x => x.CaseTypeId)
                .GreaterThan(0).WithMessage("Case type is required");

            RuleFor(x => x.PropellantId)
                .GreaterThan(0).WithMessage("Propellant is required");

            RuleFor(x => x.CompatibilityId)
                .GreaterThan(0).WithMessage("Compatibility is required");

            RuleFor(x => x.HazardDivisionId)
                .GreaterThan(0).WithMessage("Hazard division is required");

            // Optional field validations
            RuleFor(x => x.NatureOptionId)
                .GreaterThan(0).When(x => x.NatureOptionId.HasValue)
                .WithMessage("Nature option ID must be greater than 0 when provided");

            RuleFor(x => x.PrimaryPurposId)
                .GreaterThan(0).When(x => x.PrimaryPurposId.HasValue)
                .WithMessage("Primary purpose ID must be greater than 0 when provided");

            RuleFor(x => x.ProjectileColorId)
                .GreaterThan(0).When(x => x.ProjectileColorId.HasValue)
                .WithMessage("Projectile color ID must be greater than 0 when provided");

            RuleFor(x => x.ProjectailMaterialId)
                .GreaterThan(0).When(x => x.ProjectailMaterialId.HasValue)
                .WithMessage("Projectile material ID must be greater than 0 when provided");
        }
    }
}

