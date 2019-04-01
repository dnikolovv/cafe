using FluentValidation;

namespace Cafe.Core.OrderContext.Commands
{
    public class OrderToGoValidator : AbstractValidator<OrderToGo>
    {
        public OrderToGoValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
            RuleFor(c => c.ItemNumbers).NotNull();
            RuleFor(c => c.ItemNumbers).NotEmpty();
        }
    }
}
