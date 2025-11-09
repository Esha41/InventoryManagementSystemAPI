using FluentValidation;
using Ettad.Notification.Service.Dtos;

namespace Ettad.Notification.Service.Validators
{
    public class CreateNotificationDtoValidator : AbstractValidator<CreateNotificationDto>
    {
        public CreateNotificationDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(500).WithMessage("Title must not exceed 500 characters");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message is required")
                .MaximumLength(2000).WithMessage("Message must not exceed 2000 characters");

            RuleFor(x => x.EntityType)
                .MaximumLength(100).WithMessage("EntityType must not exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.EntityType));

            // If EntityType is provided, EntityId should also be provided (and vice versa)
            RuleFor(x => x)
                .Must(x => (string.IsNullOrEmpty(x.EntityType) && !x.EntityId.HasValue) || 
                           (!string.IsNullOrEmpty(x.EntityType) && x.EntityId.HasValue && x.EntityId.Value > 0))
                .WithMessage("EntityType and EntityId must both be provided together, or both be null for system notifications");

            RuleFor(x => x.EntityId)
                .GreaterThan(0).WithMessage("EntityId must be greater than 0")
                .When(x => x.EntityId.HasValue);

            RuleFor(x => x)
                .Must(x => (x.UserIds != null && x.UserIds.Any()) || (x.RoleIds != null && x.RoleIds.Any()))
                .WithMessage("At least one UserId or RoleId must be specified");
        }
    }
}

