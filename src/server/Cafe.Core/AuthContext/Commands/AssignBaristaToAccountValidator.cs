using FluentValidation;

namespace Cafe.Core.AuthContext.Commands
{
    public class AssignBaristaToAccountValidator : AbstractValidator<AssignBaristaToAccount>
    {
        public AssignBaristaToAccountValidator()
        {
            RuleFor(c => c.BaristaId).NotEmpty();
            RuleFor(c => c.AccountId).NotEmpty();
        }
    }
}
