using AutoMapper;
using Cafe.Core;
using Cafe.Core.AuthContext;
using Cafe.Core.AuthContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Domain.Views;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using Marten;
using Microsoft.AspNetCore.Identity;
using Optional;
using Optional.Async;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.AuthContext.CommandHandlers
{
    public class LoginHandler : BaseAuthHandler<Login>, ICommandHandler<Login, JwtView>
    {
        public LoginHandler(
            UserManager<User> userManager,
            IJwtFactory jwtFactory,
            IMapper mapper,
            IValidator<Login> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus)
            : base(userManager, jwtFactory, mapper, validator, dbContext, documentSession, eventBus)
        {
        }

        public Task<Option<JwtView, Error>> Handle(Login command, CancellationToken cancellationToken = default) =>
            ValidateCommand(command).FlatMapAsync(cmd =>
            FindUser(command.Email).FlatMapAsync(user =>
            CheckPassword(user, command.Password).FlatMapAsync(_ =>
            GetExtraClaims(user).MapAsync(async claims =>
            GenerateJwt(user, claims)))));

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

        private Task<Option<IList<Claim>, Error>> GetExtraClaims(User user) =>
            user.SomeNotNull(Error.Validation($"You must provide a non-null user."))
                .MapAsync(u => UserManager.GetClaimsAsync(u));

        private JwtView GenerateJwt(User user, IEnumerable<Claim> extraClaims) =>
            new JwtView
            {
                TokenString = JwtFactory.GenerateEncodedToken(user.Id.ToString(), user.Email, extraClaims)
            };
    }
}