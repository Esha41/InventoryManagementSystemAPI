using Ettad.RequestManagement.Service.SupplyManagement.Dtos;
using FluentValidation;

namespace Ettad.RequestManagement.Service.SupplyManagement.Validators
{
    public class SubmitSupplyDtoValidator : AbstractValidator<SubmitSupplyDto>
    {
        public SubmitSupplyDtoValidator()
        {
            RuleFor(x => x.RecieverName)
                .NotEmpty().WithMessage("Receiver name is required.")
                .MaximumLength(255).WithMessage("Receiver name cannot exceed 255 characters.");

            RuleFor(x => x.ReceiverRankId)
                .GreaterThan(0).WithMessage("Receiver rank must be greater than zero.");

            RuleFor(x => x.RecieverMilitaryId)
                .NotEmpty().WithMessage("Receiver military ID is required.")
                .MaximumLength(255).WithMessage("Receiver military ID cannot exceed 255 characters.");
        }
    }
}

