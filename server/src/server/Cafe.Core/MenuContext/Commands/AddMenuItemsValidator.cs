using Cafe.Domain.Views;
using FluentValidation;

namespace Cafe.Core.MenuContext.Commands
{
    public class AddMenuItemsValidator : AbstractValidator<AddMenuItems>
    {
        public AddMenuItemsValidator()
        {
            RuleFor(c => c.MenuItems).NotNull();
            RuleFor(c => c.MenuItems).NotEmpty();
            RuleForEach(c => c.MenuItems).SetValidator(new MenuItemValidator());
        }
    }

    public class MenuItemValidator : AbstractValidator<MenuItemView>
    {
        public MenuItemValidator()
        {
            RuleFor(i => i.Number).GreaterThan(0);
            RuleFor(i => i.Description).NotEmpty();
            RuleFor(i => i.Price).GreaterThan(0);
        }
    }
}
