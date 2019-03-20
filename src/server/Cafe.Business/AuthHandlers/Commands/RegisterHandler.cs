using AutoMapper;
using Cafe.Core;
using Cafe.Core.Auth;
using Cafe.Core.Auth.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Models.Auth;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using Marten;
using Microsoft.AspNetCore.Identity;
using Optional;
using Optional.Async;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.AuthHandlers.Commands
{
    public class RegisterHandler : BaseAuthHandler<Register>, ICommandHandler<Register, UserModel>
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

        public Task<Option<UserModel, Error>> Handle(Register command, CancellationToken cancellationToken = default) =>
            ValidateCommand(command).FlatMapAsync(cmd =>
            CheckIfUserDoesntExist(command.Email).FlatMapAsync(async _ =>
            {
                var user = Mapper.Map<User>(command);

                var creationResult = (await UserManager.CreateAsync(user, command.Password))
                    .SomeWhen(
                        x => x.Succeeded,
                        x => Error.Validation(x.Errors.Select(e => e.Description)));

                // If the result is valid, simply disregard it and return the new user as a DTO
                return creationResult.Map(__ => Mapper.Map<UserModel>(user));
            }));

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
