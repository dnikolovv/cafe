using AutoMapper;
using Cafe.Core.BaristaContext.Commands;
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
using System.Threading.Tasks;
using IDocumentSession = Marten.IDocumentSession;

namespace Cafe.Business.BaristaContext.CommandHandlers
{
    public class HireBaristaHandler : BaseHandler<HireBarista>
    {
        public HireBaristaHandler(
            IValidator<HireBarista> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
        }

        public override Task<Option<Unit, Error>> Handle(HireBarista command) =>
            BaristaShouldNotExist(command.Id).MapAsync(_ =>
            Persist(Mapper.Map<Barista>(command)));

        private async Task<Unit> Persist(Barista barista)
        {
            DbContext.Baristas.Add(barista);
            await DbContext.SaveChangesAsync();
            return Unit.Value;
        }

        private async Task<Option<Unit, Error>> BaristaShouldNotExist(Guid id) =>
            (await DbContext
                .Baristas
                .FirstOrDefaultAsync(b => b.Id == id))
            .SomeWhen(b => b == null, Error.Conflict($"Barista {id} already exists."))
            .Map(_ => Unit.Value);
    }
}
