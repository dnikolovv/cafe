using AutoMapper;
using Cafe.Core;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Domain.Views;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using Marten;
using Optional;
using Optional.Async;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.WaiterContext.CommandHandlers
{
    public class HireWaiterHandler : BaseHandler<HireWaiter>, ICommandHandler<HireWaiter, WaiterView>
    {
        public HireWaiterHandler(
            IValidator<HireWaiter> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
        }

        public Task<Option<WaiterView, Error>> Handle(HireWaiter command, CancellationToken cancellationToken) =>
            ValidateCommand(command).FlatMapAsync(_ =>
            PersistWaiter(command).MapAsync(async waiter =>
            Mapper.Map<WaiterView>(waiter)));

        private async Task<Option<Waiter, Error>> PersistWaiter(HireWaiter command)
        {
            var waiter = Mapper.Map<Waiter>(command);
            await DbContext.AddAsync(waiter);
            await DbContext.SaveChangesAsync();
            return waiter.Some<Waiter, Error>();
        }
    }
}
