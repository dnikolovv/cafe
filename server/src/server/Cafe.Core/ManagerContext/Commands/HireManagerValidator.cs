using FluentValidation;

namespace Cafe.Core.ManagerContext.Commands
{
    public class HireManagerValidator : AbstractValidator<HireManager>
    {
        public HireManagerValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
            RuleFor(c => c.ShortName).NotNull();
            RuleFor(c => c.ShortName).NotEmpty();
        }
    }
}
