using AutoMapper;
using Cafe.Core;
using Cafe.Core.AuthContext;
using Cafe.Core.AuthContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Optional;
using Optional.Async;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using IDocumentSession = Marten.IDocumentSession;

namespace Cafe.Business.AuthContext.CommandHandlers
{
    public class AssignWaiterToAccountHandler : BaseAuthHandler<AssignWaiterToAccount>, ICommandHandler<AssignWaiterToAccount>
    {
        public AssignWaiterToAccountHandler(
            UserManager<User> userManager,
            IJwtFactory jwtFactory,
            IMapper mapper,
            IValidator<AssignWaiterToAccount> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus)
            : base(userManager, jwtFactory, mapper, validator, dbContext, documentSession, eventBus)
        {
        }

        public Task<Option<Unit, Error>> Handle(AssignWaiterToAccount command, CancellationToken cancellationToken) =>
            ValidateCommand(command).FlatMapAsync(_ =>
            AccountShouldExist(command.AccountId).FlatMapAsync(account =>
            WaiterShouldExist(command.WaiterId).MapAsync(waiter =>
            AddWaiterClaim(account, waiter.Id))));

        private Task<Option<User, Error>> AccountShouldExist(Guid accountId) =>
            UserManager
                .FindByIdAsync(accountId.ToString())
                .SomeNotNull(Error.NotFound($"No account with id {accountId} was found."));

        private Task<Option<Waiter, Error>> WaiterShouldExist(Guid waiterId) =>
            DbContext
                .Waiters
                .FirstOrDefaultAsync(w => w.Id == waiterId)
                .SomeNotNull(Error.NotFound($"No waiter with id {waiterId} was found."));

        private async Task<Unit> AddWaiterClaim(User account, Guid waiterId)
        {
            var claimToAdd = new Claim("waiterId", waiterId.ToString());

            await UserManager.AddClaimAsync(account, claimToAdd);

            return Unit.Value;
        }
    }
}
