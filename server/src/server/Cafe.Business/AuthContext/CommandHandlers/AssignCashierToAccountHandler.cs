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
    public class AssignCashierToAccountHandler : BaseAuthHandler<AssignCashierToAccount>
    {
        public AssignCashierToAccountHandler(
            UserManager<User> userManager,
            IMapper mapper,
            IValidator<AssignCashierToAccount> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus)
            : base(userManager, mapper, validator, dbContext, documentSession, eventBus)
        {
        }

        public override Task<Option<Unit, Error>> Handle(AssignCashierToAccount command) =>
            AccountShouldExist(command.AccountId).FlatMapAsync(account =>
            CashierShouldExist(command.CashierId).MapAsync(cashier =>
            ReplaceClaim(account, AuthConstants.ClaimTypes.CashierId, cashier.Id.ToString())));

        private Task<Option<Cashier, Error>> CashierShouldExist(Guid cashierId) =>
            DbContext
                .Cashiers
                .FirstOrDefaultAsync(c => c.Id == cashierId)
                .SomeNotNull(Error.NotFound($"No cashier with an id of {cashierId} was found."));
    }
}
