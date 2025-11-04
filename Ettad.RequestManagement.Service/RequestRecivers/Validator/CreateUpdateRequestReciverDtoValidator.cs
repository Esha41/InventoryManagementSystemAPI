using FluentValidation;
using Ettad.RequestManagement.Service.RequestRecivers.Dtos;

namespace Ettad.RequestManagement.Service.RequestRecivers.Validator
{
    public class CreateUpdateRequestReciverDtoValidator : AbstractValidator<CreateUpdateRequestReciverDto>
    {
        public CreateUpdateRequestReciverDtoValidator()
        {
            RuleFor(x => x.ReciverIdNo)
                .NotEmpty().WithMessage("Receiver ID number is required")
                .MaximumLength(100).WithMessage("Receiver ID number cannot exceed 100 characters");

            RuleFor(x => x.ReciverName)
                .NotEmpty().WithMessage("Receiver name is required")
                .MaximumLength(200).WithMessage("Receiver name cannot exceed 200 characters");

            RuleFor(x => x.RankId)
                .GreaterThan(0).WithMessage("Rank ID must be greater than 0");
        }
    }
}

