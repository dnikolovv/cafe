using AutoMapper;
using Cafe.Core.Auth;
using Cafe.Core.Auth.Commands;
using Cafe.Core.CQRS;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Models.Auth;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Optional;
using Optional.Async;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.Auth.Handlers
{
    public class RegisterHandler : BaseAuthHandler<Register>, ICommandHandler<Register, UserModel>
    {
        public RegisterHandler(
            IValidator<Register> validator,
            UserManager<User> userManager,
            IJwtFactory jwtFactory,
            IMapper mapper)
            : base(validator, userManager, jwtFactory, mapper)
        {
        }

        public Task<Option<UserModel, Error>> Handle(Register command, CancellationToken cancellationToken = default) =>
            ValidateCommand(command).FlatMapAsync(async cmd =>
            {
                var user = Mapper.Map<User>(command);

                var creationResult = (await UserManager.CreateAsync(user, command.Password))
                    .SomeWhen(
                        x => x.Succeeded,
                        x => Error.FromCollection(x.Errors.Select(e => e.Description)));

                return creationResult.Map(_ => Mapper.Map<UserModel>(user));
            });
    }
}
