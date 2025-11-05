using FluentValidation;
using Ettad.RequestManagement.Service.Requests.Dtos;
using Ettad.Data.Enums;

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
                .IsInEnum().WithMessage("Request status must be a valid value (Pending, Approved, Rejected)");

            RuleFor(x => x.DepotId)
                .GreaterThan(0).WithMessage("Depot ID must be greater than 0");

            RuleFor(x => x.RequestPriority)
                .IsInEnum().WithMessage("Request priority must be a valid value (High, Medium, Low)");

            RuleFor(x => x.RequestType)
                .IsInEnum().WithMessage("Request type must be a valid value (Order, Return, Discard)");

            RuleFor(x => x.RequestPurpose)
                .IsInEnum().WithMessage("Request purpose must be a valid value (Normal, Duty, Operation, Training)");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Department ID must be greater than 0");

            RuleFor(x => x.RequestReciverId)
                .GreaterThan(0).WithMessage("Request receiver ID must be greater than 0");

            RuleFor(x => x.RequesterRankId)
                .GreaterThan(0).WithMessage("Requester rank ID must be greater than 0");

            // Validate RequestDetails
            RuleForEach(x => x.RequestDetails)
                .ChildRules(detail =>
                {
                    detail.RuleFor(d => d.ItemId)
                        .GreaterThan(0).WithMessage("Item ID must be greater than 0");

                    detail.RuleFor(d => d.ItemQuantity)
                        .GreaterThan(0).WithMessage("Item quantity must be greater than 0");
                });
        }
    }
}
