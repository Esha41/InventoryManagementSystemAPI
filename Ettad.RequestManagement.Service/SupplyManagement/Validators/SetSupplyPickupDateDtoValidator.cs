using Ettad.RequestManagement.Service.SupplyManagement.Dtos;
using FluentValidation;

namespace Ettad.RequestManagement.Service.SupplyManagement.Validators
{
    public class SetSupplyPickupDateDtoValidator : AbstractValidator<SetSupplyPickupDateDto>
    {
        public SetSupplyPickupDateDtoValidator()
        {
            RuleFor(x => x.SupplyDate)
                .NotEmpty().WithMessage("Supply date is required.")
                .Must(date => date >= DateTime.Today).WithMessage("Supply date cannot be in the past.");
        }
    }
}

