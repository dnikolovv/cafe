using AutoMapper;
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
using System.Threading.Tasks;
using IDocumentSession = Marten.IDocumentSession;

namespace Cafe.Business.AuthContext.CommandHandlers
{
    public class AssignWaiterToAccountHandler : BaseAuthHandler<AssignWaiterToAccount>
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

        public override Task<Option<Unit, Error>> Handle(AssignWaiterToAccount command) =>
            AccountShouldExist(command.AccountId).FlatMapAsync(account =>
            WaiterShouldExist(command.WaiterId).MapAsync(waiter =>
            AddClaim(account, AuthConstants.ClaimTypes.WaiterId, waiter.Id.ToString())));

        private Task<Option<Waiter, Error>> WaiterShouldExist(Guid waiterId) =>
            DbContext
                .Waiters
                .FirstOrDefaultAsync(w => w.Id == waiterId)
                .SomeNotNull(Error.NotFound($"No waiter with id {waiterId} was found."));
    }
}
