using FluentValidation;

namespace Cafe.Core.CashierContext.Commands
{
    public class HireCashierValidator : AbstractValidator<HireCashier>
    {
        public HireCashierValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
            RuleFor(c => c.ShortName).NotNull();
            RuleFor(c => c.ShortName).NotEmpty();
        }
    }
}
