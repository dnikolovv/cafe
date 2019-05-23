using AutoMapper;
using Cafe.Core.TableContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Domain.Repositories;
using FluentValidation;
using MediatR;
using Optional;
using Optional.Async;
using System.Threading.Tasks;

namespace Cafe.Business.TableContext.CommandHandlers
{
    public class AddTableHandler : BaseTableHandler<AddTable>
    {
        public AddTableHandler(
            IValidator<AddTable> validator,
            IEventBus eventBus,
            IMapper mapper,
            ITableRepository tableRepository)
            : base(validator, eventBus, mapper, tableRepository)
        {
        }

        public override Task<Option<Unit, Error>> Handle(AddTable command) =>
            CheckIfNumberIsNotTaken(command).MapAsync(
            PersistTable);

        private Task<Unit> PersistTable(Table table) =>
            TableRepository.Add(table);

        private async Task<Option<Table, Error>> CheckIfNumberIsNotTaken(AddTable command)
        {
            var table = await TableRepository
                .GetByNumber(command.Number);

            return table
                .SomeWhen(t => !t.HasValue, Error.Conflict($"Table number '{command.Number}' is already taken."))
                .Map(_ => Mapper.Map<Table>(command));
        }
    }
}
