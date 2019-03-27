using AutoMapper;
using Cafe.Core;
using Cafe.Core.AuthContext;
using Cafe.Core.AuthContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using Marten;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Optional;
using Optional.Async;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.AuthContext.CommandHandlers
{
    public class RegisterHandler : BaseAuthHandler<Register>, ICommandHandler<Register>
    {
        public RegisterHandler(
            UserManager<User> userManager,
            IJwtFactory jwtFactory,
            IMapper mapper,
            IValidator<Register> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus)
            : base(userManager, jwtFactory, mapper, validator, dbContext, documentSession, eventBus)
        {
        }

        public Task<Option<Unit, Error>> Handle(Register command, CancellationToken cancellationToken = default) =>
            ValidateCommand(command).FlatMapAsync(_ =>
            CheckIfUserDoesntExist(command.Email).FlatMapAsync(__ =>
            PersistUser(command)));

        private async Task<Option<Unit, Error>> PersistUser(Register command)
        {
            var user = Mapper.Map<User>(command);

            var creationResult = (await UserManager.CreateAsync(user, command.Password))
                .SomeWhen(
                    x => x.Succeeded,
                    x => Error.Validation(x.Errors.Select(e => e.Description)));

            return creationResult
                .Map(_ => Unit.Value);
        }

        private async Task<Option<User, Error>> CheckIfUserDoesntExist(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);

            return user
                .SomeWhen(
                    u => u == null,
                    Error.Conflict($"User with email {email} already exists."));
        }
    }
}
