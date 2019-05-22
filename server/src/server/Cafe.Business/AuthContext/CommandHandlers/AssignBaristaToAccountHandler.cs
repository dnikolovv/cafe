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
    public class AssignBaristaToAccountHandler : BaseAuthHandler<AssignBaristaToAccount>
    {
        public AssignBaristaToAccountHandler(
            UserManager<User> userManager,
            IMapper mapper,
            IValidator<AssignBaristaToAccount> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus)
            : base(userManager, mapper, validator, dbContext, documentSession, eventBus)
        {
        }

        public override Task<Option<Unit, Error>> Handle(AssignBaristaToAccount command) =>
            AccountShouldExist(command.AccountId).FlatMapAsync(user =>
            BaristaShouldExist(command.BaristaId).MapAsync(barista =>
            ReplaceClaim(user, AuthConstants.ClaimTypes.BaristaId, barista.Id.ToString())));

        private async Task<Option<Barista, Error>> BaristaShouldExist(Guid baristaId) =>
            (await DbContext
                .Baristas
                .FirstOrDefaultAsync(b => b.Id == baristaId))
            .SomeNotNull(Error.NotFound($"Barista {baristaId} was not found."));
    }
}
