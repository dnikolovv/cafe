using FluentValidation;

namespace Cafe.Core.TabContext.Commands
{
    public class RejectMenuItemsValidator : AbstractValidator<RejectMenuItems>
    {
        public RejectMenuItemsValidator()
        {
            RuleFor(c => c.TabId).NotEmpty();
            RuleFor(c => c.ItemNumbers).NotNull();
            RuleFor(c => c.ItemNumbers).NotEmpty();
        }
    }
}
