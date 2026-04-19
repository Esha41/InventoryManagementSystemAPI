using Ettad.RequestManagement.Service.SupplyManagement.Dtos;
using FluentValidation;

namespace Ettad.RequestManagement.Service.SupplyManagement.Validators
{
    public class SubmitSupplyDtoValidator : AbstractValidator<SubmitSupplyDto>
    {
        public SubmitSupplyDtoValidator()
        {
            RuleFor(x => x.ReceiverEmployeeId)
                .GreaterThan(0).WithMessage("Receiver employee is required.");
        }
    }
}

