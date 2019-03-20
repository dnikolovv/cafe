using AutoMapper;
using Cafe.Core.Auth;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using Marten;
using Microsoft.AspNetCore.Identity;

namespace Cafe.Business.AuthHandlers.Commands
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
            : base(validator, dbContext, documentSession, eventBus)
        {
            UserManager = userManager;
            JwtFactory = jwtFactory;
            Mapper = mapper;
        }

        protected UserManager<User> UserManager { get; }
        protected IJwtFactory JwtFactory { get; }
        protected IMapper Mapper { get; }
    }
}
