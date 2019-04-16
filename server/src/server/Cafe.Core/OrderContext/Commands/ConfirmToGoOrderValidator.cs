using FluentValidation;

namespace Cafe.Core.OrderContext.Commands
{
    public class ConfirmToGoOrderValidator : AbstractValidator<ConfirmToGoOrder>
    {
        public ConfirmToGoOrderValidator()
        {
            RuleFor(c => c.OrderId).NotEmpty();
            RuleFor(c => c.PricePaid).GreaterThan(0);
        }
    }
}
