using AutoMapper;
using Cafe.Core.TableContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Events;
using Cafe.Domain.Repositories;
using FluentValidation;
using MediatR;
using Optional;
using Optional.Async;
using System.Threading.Tasks;

namespace Cafe.Business.TableContext.CommandHandlers
{
    public class CallWaiterHandler : BaseTableHandler<CallWaiter>
    {
        public CallWaiterHandler(
            IValidator<CallWaiter> validator,
            IEventBus eventBus,
            IMapper mapper,
            ITableRepository tableRepository)
            : base(validator, eventBus, mapper, tableRepository)
        {
        }

        public override Task<Option<Unit, Error>> Handle(CallWaiter command) =>
            TableShouldExist(command.TableNumber).FlatMapAsync(table =>
            TableShouldHaveAWaiterAssigned(table).MapAsync(waiter =>
            PublishEvents(table.Id, new WaiterCalled { TableNumber = table.Number, WaiterId = waiter.Id })));
    }
}
