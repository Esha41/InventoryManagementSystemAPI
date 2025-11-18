using FluentValidation;
using Ettad.RequestManagement.Service.SupplyManagement.Dtos;

namespace Ettad.RequestManagement.Service.SupplyManagement.Validators
{
    public class UpdateSupplyDtoValidator : AbstractValidator<UpdateSupplyDto>
    {
        public UpdateSupplyDtoValidator()
        {
            RuleFor(x => x.RecieverName)
                .NotEmpty().WithMessage("Reciever Name is required.")
                .MaximumLength(255).WithMessage("Reciever Name cannot exceed 255 characters.");

            RuleFor(x => x.ReceiverRankId)
                .GreaterThan(0).WithMessage("Receiver Rank Id must be greater than zero.");

            RuleFor(x => x.RecieverMilitaryId)
                .NotEmpty().WithMessage("Reciever Military Id is required.")
                .MaximumLength(255).WithMessage("Reciever Military Id cannot exceed 255 characters.");
        }
    }
}
