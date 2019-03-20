using FluentValidation;

namespace Cafe.Core.WaiterContext.Commands
{
    public class HireWaiterValidator : AbstractValidator<HireWaiter>
    {
        public HireWaiterValidator()
        {
            RuleFor(c => c.ShortName).NotNull();
            RuleFor(c => c.ShortName).NotEmpty();
        }
    }
}
