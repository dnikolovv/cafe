using FluentValidation;

namespace Cafe.Core.TabContext.Commands
{
    public class ServeMenuItemsValidator : AbstractValidator<ServeMenuItems>
    {
        public ServeMenuItemsValidator()
        {
            RuleFor(c => c.TabId).NotEmpty();
            RuleFor(c => c.ItemNumbers).NotNull();
            RuleFor(c => c.ItemNumbers).NotEmpty();
        }
    }
}
