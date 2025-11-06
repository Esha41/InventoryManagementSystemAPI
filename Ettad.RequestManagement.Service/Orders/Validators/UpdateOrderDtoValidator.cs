using FluentValidation;
using Ettad.Data.Enums;
using Ettad.RequestManagement.Service.Orders.Dto;

namespace Ettad.RequestManagement.Service.Orders.Validators
{
    public class UpdateOrderDtoValidator : AbstractValidator<UpdateOrderDto>
    {
        public UpdateOrderDtoValidator()
        {
            // BaseRequest Properties Validation
           

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Invalid priority");

           

           
           
            // Request Items Validation
            RuleFor(x => x.RequestItems)
                .NotEmpty().WithMessage("At least one request item is required");

            RuleForEach(x => x.RequestItems)
                .SetValidator(new CreateUpdateRequestItemDtoValidator());
        }
    }
}

