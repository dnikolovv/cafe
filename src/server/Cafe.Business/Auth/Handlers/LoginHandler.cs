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
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.Auth.Handlers
{
    public class LoginHandler : BaseAuthHandler<Login>, ICommandHandler<Login, JwtModel>
    {
        public LoginHandler(
            IValidator<Login> validator,
            UserManager<User> userManager,
            IJwtFactory jwtFactory,
            IMapper mapper)
            : base(validator, userManager, jwtFactory, mapper)
        {
        }

        public Task<Option<JwtModel, Error>> Handle(Login command, CancellationToken cancellationToken = default) =>
            ValidateCommand(command).FlatMapAsync(cmd =>
            FindUser(command.Email).FlatMapAsync(user =>
            CheckPassword(user, command.Password).MapAsync(async _ =>
            GenerateJwt(user))));

        private async Task<Option<bool, Error>> CheckPassword(User user, string password)
        {
            var passwordIsValid = await UserManager
                .CheckPasswordAsync(user, password);

            var result = passwordIsValid
                .SomeWhen(isValid => isValid == true, Error.Unauthorized("Invalid credentials."));

            return result;
        }

        private Task<Option<User, Error>> FindUser(string email) =>
            UserManager
                .FindByEmailAsync(email)
                .SomeNotNull(Error.NotFound($"No user with email {email} was found."));

        private JwtModel GenerateJwt(User user) =>
            new JwtModel
            {
                TokenString = JwtFactory.GenerateEncodedToken(user.Id, user.Email, Enumerable.Empty<Claim>())
            };
    }
}