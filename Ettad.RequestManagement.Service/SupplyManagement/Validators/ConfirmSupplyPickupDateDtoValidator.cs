using Ettad.RequestManagement.Service.SupplyManagement.Dtos;
using FluentValidation;

namespace Ettad.RequestManagement.Service.SupplyManagement.Validators
{
    public class ConfirmSupplyPickupDateDtoValidator : AbstractValidator<ConfirmSupplyPickupDateDto>
    {
        public ConfirmSupplyPickupDateDtoValidator()
        {
            RuleFor(x => x.SupplyDate)
                .NotEmpty().WithMessage("Supply date is required.");
        }
    }
}

