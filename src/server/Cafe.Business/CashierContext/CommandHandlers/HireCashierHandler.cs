using AutoMapper;
using Cafe.Core;
using Cafe.Core.CashierContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Optional;
using Optional.Async;
using System;
using System.Threading;
using System.Threading.Tasks;
using IDocumentSession = Marten.IDocumentSession;

namespace Cafe.Business.CashierContext.CommandHandlers
{
    public class HireCashierHandler : BaseHandler<HireCashier>, ICommandHandler<HireCashier>
    {
        public HireCashierHandler(
            IValidator<HireCashier> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
        }

        public Task<Option<Unit, Error>> Handle(HireCashier command, CancellationToken cancellationToken) =>
            ValidateCommand(command).FlatMapAsync(_ =>
            CashierShouldntExist(command.Id).MapAsync(__ =>
            PersistCashier(command)));

        private async Task<Option<Unit, Error>> CashierShouldntExist(Guid cashierId) =>
            (await DbContext
                .Cashiers
                .FirstOrDefaultAsync(c => c.Id == cashierId))
                .SomeWhen(c => c == null, Error.Conflict($"Cashier {cashierId} already exists."))
                .Map(_ => Unit.Value);

        private async Task<Unit> PersistCashier(HireCashier command)
        {
            var cashier = Mapper.Map<Cashier>(command);

            DbContext.Add(cashier);
            await DbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
