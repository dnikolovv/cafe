using FluentValidation;

namespace Cafe.Core.AuthContext.Commands
{
    public class AssignWaiterToAccountValidator : AbstractValidator<AssignWaiterToAccount>
    {
        public AssignWaiterToAccountValidator()
        {
            RuleFor(c => c.AccountId).NotNull();
            RuleFor(c => c.AccountId).NotEmpty();
            RuleFor(c => c.WaiterId).NotEmpty();
        }
    }
}
