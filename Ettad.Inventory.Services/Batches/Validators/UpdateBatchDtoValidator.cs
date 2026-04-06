using Ettad.Inventory.Service.Batches.Dtos;
using FluentValidation;

namespace Ettad.Inventory.Service.Batches.Validators
{
    public class UpdateBatchDtoValidator : AbstractValidator<UpdateBatchDto>
    {
        public UpdateBatchDtoValidator()
        {
            RuleFor(x => x.BatchNumber)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .Must(s => !string.IsNullOrWhiteSpace(s))
                .WithMessage("Batch number is required.")
                .MaximumLength(500);
        }
    }
}
