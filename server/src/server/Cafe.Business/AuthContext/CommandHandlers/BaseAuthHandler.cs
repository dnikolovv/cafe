using AutoMapper;
using Cafe.Core;
using Cafe.Core.AuthContext;
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
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cafe.Business.AuthContext.CommandHandlers
{
    public abstract class BaseAuthHandler<TCommand> : BaseHandler<TCommand>
        where TCommand : ICommand
    {
        public BaseAuthHandler(
            UserManager<User> userManager,
            IMapper mapper,
            IValidator<TCommand> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
            UserManager = userManager;
        }

        protected UserManager<User> UserManager { get; }

        protected Task<Option<User, Error>> AccountShouldExist(Guid accountId) =>
            UserManager
                .FindByIdAsync(accountId.ToString())
                .SomeNotNull(Error.NotFound($"No account with id {accountId} was found."));

        protected async Task<Unit> ReplaceClaim(User account, string claimType, string claimValue)
        {
            var claimToReplace = (await UserManager.GetClaimsAsync(account))
                .FirstOrDefault(c => c.Type == claimType);

            var claimToAdd = new Claim(claimType, claimValue);

            if (claimToReplace != null)
            {
                await UserManager.ReplaceClaimAsync(account, claimToReplace, claimToAdd);
            }
            else
            {
                await UserManager.AddClaimAsync(account, claimToAdd);
            }

            return Unit.Value;
        }
    }
}
