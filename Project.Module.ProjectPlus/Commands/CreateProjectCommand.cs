using FluentValidation;
using MediatR;
using Project.ResponseHandler.ResponseModel;

namespace Project.Module.ProjectPlus.Commands
{
    public class CreateProjectCommand : IRequest<ApiResponse>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public decimal Budget { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Project name is required")
                .MaximumLength(100).WithMessage("Project name cannot exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required")
                .Must(date => date >= DateTime.Today).WithMessage("Start date must be today or in the future");

            RuleFor(x => x.EndDate)
                .Must((cmd, endDate) => !endDate.HasValue || endDate.Value > cmd.StartDate)
                .WithMessage("End date must be after start date");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required")
                .MaximumLength(50).WithMessage("Status cannot exceed 50 characters");

            RuleFor(x => x.Budget)
                .GreaterThanOrEqualTo(0).WithMessage("Budget must be greater than or equal to 0");
        }
    }
} 