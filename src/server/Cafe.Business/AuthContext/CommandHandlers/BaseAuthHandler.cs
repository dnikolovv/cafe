using AutoMapper;
using Cafe.Core.AuthContext;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using Marten;
using Microsoft.AspNetCore.Identity;

namespace Cafe.Business.AuthContext.CommandHandlers
{
    public abstract class BaseAuthHandler<TCommand> : BaseHandler<TCommand>
    {
        public BaseAuthHandler(
            UserManager<User> userManager,
            IJwtFactory jwtFactory,
            IMapper mapper,
            IValidator<TCommand> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
            UserManager = userManager;
            JwtFactory = jwtFactory;
        }

        protected UserManager<User> UserManager { get; }
        protected IJwtFactory JwtFactory { get; }
    }
}
