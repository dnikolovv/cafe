using FluentValidation;
using System;

namespace Cafe.Core.Tab.Commands
{
    public class OpenTabValidator : AbstractValidator<OpenTab>
    {
        public OpenTabValidator()
        {
            RuleFor(c => c.Id).NotEqual(Guid.Empty);
            RuleFor(c => c.ClientName).NotNull();
            RuleFor(c => c.ClientName).NotEmpty();
        }
    }
}
