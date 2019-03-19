using FluentValidation;

namespace Cafe.Core.Auth.Commands
{
    public class LoginValidator : AbstractValidator<Login>
    {
        public LoginValidator()
        {
            RuleFor(c => c.Email).NotNull();
            RuleFor(c => c.Email).EmailAddress();
            RuleFor(c => c.Password).NotNull();
        }
    }
}
