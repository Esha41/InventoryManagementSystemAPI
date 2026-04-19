using FluentValidation;
using Ettad.RequestManagement.Service.SupplyManagement.Dtos;

namespace Ettad.RequestManagement.Service.SupplyManagement.Validators
{
    public class UpdateSupplyDtoValidator : AbstractValidator<UpdateSupplyDto>
    {
        public UpdateSupplyDtoValidator()
        {
            RuleFor(x => x.ReceiverEmployeeId)
                .GreaterThan(0).WithMessage("Receiver employee is required.");
        }
    }
}
