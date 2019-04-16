using FluentValidation;

namespace Cafe.Core.TabContext.Commands
{
    public class OrderMenuItemsValidator : AbstractValidator<OrderMenuItems>
    {
        public OrderMenuItemsValidator()
        {
            RuleFor(c => c.TabId).NotEmpty();
            RuleFor(c => c.ItemNumbers).NotEmpty();
        }
    }
}
