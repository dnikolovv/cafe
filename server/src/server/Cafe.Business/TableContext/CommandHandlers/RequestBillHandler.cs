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
    public class RequestBillHandler : BaseTableHandler<RequestBill>
    {
        public RequestBillHandler(
            IValidator<RequestBill> validator,
            IEventBus eventBus,
            IMapper mapper,
            ITableRepository tableRepository)
            : base(validator, eventBus, mapper, tableRepository)
        {
        }

        public override Task<Option<Unit, Error>> Handle(RequestBill command) =>
            TableShouldExist(command.TableNumber).FlatMapAsync(table =>
            TableShouldHaveAWaiterAssigned(table).MapAsync(waiter =>
            PublishEvents(table.Id, new BillRequested { TableNumber = table.Number, WaiterId = waiter.Id })));
    }
}
