using FluentValidation;
using Ettad.RequestManagement.Service.Requests.Dtos;

namespace Ettad.RequestManagement.Service.Requests.Validator
{
    public class CreateUpdateRequestDtoValidator : AbstractValidator<CreateUpdateRequestDto>
    {
        public CreateUpdateRequestDtoValidator()
        {
            RuleFor(x => x.RequestNo)
                .NotEmpty().WithMessage("Request number is required")
                .MaximumLength(500).WithMessage("Request number cannot exceed 500 characters");

            RuleFor(x => x.RequestDate)
                .NotEmpty().WithMessage("Request date is required");

            RuleFor(x => x.RequestStatus)
                .NotEmpty().WithMessage("Request status is required");

            RuleFor(x => x.DepotId)
                .GreaterThan(0).WithMessage("Depot ID must be greater than 0");

            RuleFor(x => x.RequestPriority)
                .NotEmpty().WithMessage("Request priority is required");

            RuleFor(x => x.RequestKind)
                .NotEmpty().WithMessage("Request kind is required");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Department ID must be greater than 0");

            RuleFor(x => x.RequestReciverId)
                .GreaterThan(0).WithMessage("Request receiver ID must be greater than 0");
        }
    }
}
