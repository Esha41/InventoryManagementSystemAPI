using FluentValidation;
using Ettad.Inventory.Service.AllowanceItems.Dtos;
using Ettad.Data.Enums;

namespace Ettad.Inventory.Service.AllowanceItems.Validators
{
    public class BulkCreateAllowanceItemDtoValidator : AbstractValidator<BulkCreateAllowanceItemDto>
    {
        public BulkCreateAllowanceItemDtoValidator()
        {
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Department is required");

            RuleFor(x => x.Year)
                .NotEmpty().WithMessage("Year is required")
                .InclusiveBetween(1900, 5000).WithMessage("Year must be between 1900 and 5000");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Items list cannot be empty");

            RuleForEach(x => x.Items)
                .SetValidator(new BulkAllowanceItemDtoValidator());
        }
    }

    public class BulkAllowanceItemDtoValidator : AbstractValidator<BulkAllowanceItemDto>
    {
        public BulkAllowanceItemDtoValidator()
        {
            RuleFor(x => x.ItemId)
                .GreaterThan(0).WithMessage("Item is required");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");
        }
    }
}

