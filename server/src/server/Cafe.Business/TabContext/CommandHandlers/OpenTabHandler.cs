using AutoMapper;
using Cafe.Core;
using Cafe.Core.TabContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Domain.Repositories;
using FluentValidation;
using MediatR;
using Optional;
using Optional.Async;
using System.Threading.Tasks;

namespace Cafe.Business.TabContext.CommandHandlers
{
    public class OpenTabHandler : BaseTabHandler<OpenTab>
    {
        private readonly ITabViewRepository _tabViewRepository;
        private readonly ITableRepository _tableRepository;

        public OpenTabHandler(
            ITabRepository tabRepository,
            IMenuItemsService menuItemsService,
            IValidator<OpenTab> validator,
            IEventBus eventBus,
            IMapper mapper,
            ITabViewRepository tabViewRepository,
            ITableRepository tableRepository)
            : base(tabRepository, menuItemsService, validator, eventBus, mapper)
        {
            _tabViewRepository = tabViewRepository;
            _tableRepository = tableRepository;
        }

        public override Task<Option<Unit, Error>> Handle(OpenTab command) =>
            TabShouldNotExist(command.Id).FlatMapAsync(tab =>
            TableShouldNotBeTaken(command.TableNumber).FlatMapAsync(tableNumber =>
            TheTableShouldHaveAWaiterAssigned(tableNumber).MapAsync(waiter =>
            PublishEvents(tab.Id, tab.OpenTab(command.CustomerName, waiter.ShortName, command.TableNumber)))));

        private async Task<Option<int, Error>> TableShouldNotBeTaken(int tableNumber)
        {
            var thereIsATabOnTable = (await _tabViewRepository
                .GetTabs(t => t.IsOpen && t.TableNumber == tableNumber))
                .Count > 0;

            return thereIsATabOnTable
                .SomeWhen(isTaken => isTaken == false, Error.Conflict($"Table {tableNumber} is already taken."))
                .Map(_ => tableNumber);
        }

        private async Task<Option<Waiter, Error>> TheTableShouldHaveAWaiterAssigned(int tableNumber)
        {
            var table = await _tableRepository
                .GetByNumber(tableNumber);

            return table
                .WithException(Error.NotFound($"No table with number {tableNumber} was found."))
                .FlatMap(t => t
                    .Waiter
                    .SomeNotNull(Error.Validation($"Table {tableNumber} does not have a waiter assigned.")));
        }
    }
}
