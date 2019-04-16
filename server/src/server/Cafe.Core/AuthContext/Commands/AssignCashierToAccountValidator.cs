using FluentValidation;

namespace Cafe.Core.AuthContext.Commands
{
    public class AssignCashierToAccountValidator : AbstractValidator<AssignCashierToAccount>
    {
        public AssignCashierToAccountValidator()
        {
            RuleFor(c => c.AccountId).NotEmpty();
            RuleFor(c => c.CashierId).NotEmpty();
        }
    }
}
