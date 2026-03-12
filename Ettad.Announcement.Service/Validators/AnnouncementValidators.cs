using FluentValidation;
using Ettad.Announcement.Service.Dtos;

namespace Ettad.Announcement.Service.Validators
{
    public class CreateAnnouncementDtoValidator : AbstractValidator<CreateAnnouncementDto>
    {
        public CreateAnnouncementDtoValidator()
        {
            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message is required")
                .MaximumLength(500).WithMessage("Message cannot exceed 500 characters");

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Invalid priority value");

            RuleFor(x => x.DeliveryType)
                .IsInEnum().WithMessage("Invalid delivery type value");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required");

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .When(x => x.EndDate.HasValue)
                .WithMessage("End date cannot be before start date");
        }
    }

    public class UpdateAnnouncementDtoValidator : AbstractValidator<UpdateAnnouncementDto>
    {
        public UpdateAnnouncementDtoValidator()
        {
            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message cannot be empty")
                .MaximumLength(500).WithMessage("Message cannot exceed 500 characters")
                .When(x => x.Message != null);

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Invalid priority value")
                .When(x => x.Priority.HasValue);

            RuleFor(x => x.DeliveryType)
                .IsInEnum().WithMessage("Invalid delivery type value")
                .When(x => x.DeliveryType.HasValue);
        }
    }
}
