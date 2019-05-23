using AutoMapper;
using Cafe.Core.AuthContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Domain.Repositories;
using FluentValidation;
using MediatR;
using Optional;
using Optional.Async;
using System.Threading.Tasks;

namespace Cafe.Business.AuthContext.CommandHandlers
{
    public class RegisterHandler : BaseAuthHandler<Register>
    {
        public RegisterHandler(
            IValidator<Register> validator,
            IEventBus eventBus,
            IMapper mapper,
            IUserRepository userRepository)
            : base(validator, eventBus, mapper, userRepository)
        {
        }

        public override Task<Option<Unit, Error>> Handle(Register command) =>
            CheckIfUserDoesntExist(command.Email).FlatMapAsync(__ =>
            PersistUser(command));

        private Task<Option<Unit, Error>> PersistUser(Register command)
        {
            var user = Mapper.Map<User>(command);
            return UserRepository.Register(user, command.Password);
        }

        private async Task<Option<User, Error>> CheckIfUserDoesntExist(string email)
        {
            var user = await UserRepository.GetByEmail(email);

            return user
                .SomeWhen(u => !u.HasValue)
                .Flatten()
                .WithException(Error.Conflict($"User with email {email} already exists."));
        }
    }
}
