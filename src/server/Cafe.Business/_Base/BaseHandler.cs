using Cafe.Core.CQRS;
using Cafe.Domain;
using FluentValidation;
using Optional;
using Optional.Async;
using System;
using System.Linq;

namespace Cafe.Business
{
    public abstract class BaseHandler<TCommand>
    {
        public BaseHandler(IValidator<TCommand> validator)
        {
            Validator = validator ??
                throw new InvalidOperationException(
                    "Tried to instantiate a command handler without a validator." +
                    "Did you forget to add one?");
        }

        protected IValidator<TCommand> Validator { get; }

        protected Option<TCommand, Error> ValidateCommand(TCommand command)
        {
            var validationResult = Validator.Validate(command);

            return validationResult
                .SomeWhen(
                    r => r.IsValid,
                    r => Error.Validation(r.Errors.Select(e => e.ErrorMessage)))

                // If the validation result is successfull, disregard it and simply return the command
                .Map(_ => command);
        }
    }
}
