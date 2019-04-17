using AutoMapper;
using Cafe.Core.TableContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Optional;
using Optional.Async;
using System.Threading.Tasks;
using IDocumentSession = Marten.IDocumentSession;

namespace Cafe.Business.TableContext.CommandHandlers
{
    public class AddTableHandler : BaseHandler<AddTable>
    {
        public AddTableHandler(
            IValidator<AddTable> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
        }

        public override Task<Option<Unit, Error>> Handle(AddTable command) =>
            CheckIfNumberIsNotTaken(command).MapAsync(
            PersistTable);

        private async Task<Unit> PersistTable(Table table)
        {
            DbContext.Add(table);
            await DbContext.SaveChangesAsync();
            return Unit.Value;
        }

        private async Task<Option<Table, Error>> CheckIfNumberIsNotTaken(AddTable command)
        {
            var table = await DbContext
                .Tables
                .FirstOrDefaultAsync(t => t.Number == command.Number);

            return table
                .SomeWhen(t => t == null, Error.Conflict($"Table number '{command.Number}' is already taken."))
                .Map(_ => Mapper.Map<Table>(command));
        }
    }
}
