using FluentValidation;
using MediatR;
using Project.ResponseHandler.ResponseModel;

namespace Project.Module.ProjectPlus.Queries
{
    public class GetProjectQuery : IRequest<ApiResponse>
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? StartDateFrom { get; set; }
        public DateTime? StartDateTo { get; set; }
    }

    public class GetProjectQueryValidator : AbstractValidator<GetProjectQuery>
    {
        public GetProjectQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).When(x => x.HasValue)
                .WithMessage("Id must be greater than 0");

            RuleFor(x => x.Name)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage("Name cannot exceed 100 characters");

            RuleFor(x => x.Status)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.Status))
                .WithMessage("Status cannot exceed 50 characters");

            RuleFor(x => x.StartDateTo)
                .GreaterThanOrEqualTo(x => x.StartDateFrom)
                .When(x => x.StartDateFrom.HasValue && x.StartDateTo.HasValue)
                .WithMessage("Start date to must be greater than or equal to start date from");
        }
    }
} 