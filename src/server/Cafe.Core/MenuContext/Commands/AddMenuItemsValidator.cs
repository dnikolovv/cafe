using FluentValidation;

namespace Cafe.Core.MenuContext.Commands
{
    public class AddMenuItemsValidator : AbstractValidator<AddMenuItems>
    {
        public AddMenuItemsValidator()
        {
            RuleFor(c => c.MenuItems).NotNull();
            RuleFor(c => c.MenuItems).NotEmpty();
        }
    }
}
