using AutoMapper;
using Cafe.Core;
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
using System.Threading;
using System.Threading.Tasks;
using IDocumentSession = Marten.IDocumentSession;

namespace Cafe.Business.AuthContext.CommandHandlers
{
    public class AssignManagerToAccountHandler : BaseAuthHandler<AssignManagerToAccount>, ICommandHandler<AssignManagerToAccount>
    {
        public AssignManagerToAccountHandler(
            UserManager<User> userManager,
            IJwtFactory jwtFactory,
            IMapper mapper,
            IValidator<AssignManagerToAccount> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus)
            : base(userManager, jwtFactory, mapper, validator, dbContext, documentSession, eventBus)
        {
        }

        public Task<Option<Unit, Error>> Handle(AssignManagerToAccount command, CancellationToken cancellationToken) =>
            ValidateCommand(command).FlatMapAsync(_ =>
            AccountShouldExist(command.AccountId).FlatMapAsync(account =>
            ManagerShouldExist(command.ManagerId).MapAsync(manager =>
            AddClaim(account, AuthConstants.ClaimTypes.ManagerId, manager.Id.ToString()))));

        private Task<Option<Manager, Error>> ManagerShouldExist(Guid managerId) =>
            DbContext
                .Managers
                .FirstOrDefaultAsync(m => m.Id == managerId)
                .SomeNotNull(Error.NotFound($"No manager with an id of {managerId} was found."));
    }
}
