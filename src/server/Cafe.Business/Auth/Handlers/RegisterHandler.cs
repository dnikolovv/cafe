using AutoMapper;
using Cafe.Core.Auth;
using Cafe.Core.Auth.Commands;
using Cafe.Core.CQRS;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Optional;
using Optional.Async;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.Auth.Handlers
{
    public class RegisterHandler : BaseHandler, ICommandHandler<Register, UserModel>
    {
        public RegisterHandler(
            UserManager<User> userManager,
            IJwtFactory jwtFactory,
            IMapper mapper)
            : base(userManager, jwtFactory, mapper)
        {
        }

        public async Task<Option<UserModel, Error>> Handle(Register model, CancellationToken cancellationToken = default)
        {
            var user = Mapper.Map<User>(model);

            var creationResult = (await UserManager.CreateAsync(user, model.Password))
                .SomeWhen(
                    x => x.Succeeded,
                    x => Error.FromCollection(x.Errors.Select(e => e.Description)));

            return creationResult.Map(_ => Mapper.Map<UserModel>(user));
        }
    }
}
