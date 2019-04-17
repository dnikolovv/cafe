using FluentValidation;

namespace Cafe.Core.OrderContext.Commands
{
    public class CompleteToGoOrderValidator : AbstractValidator<CompleteToGoOrder>
    {
        public CompleteToGoOrderValidator()
        {
            RuleFor(c => c.OrderId).NotEmpty();
        }
    }
}
