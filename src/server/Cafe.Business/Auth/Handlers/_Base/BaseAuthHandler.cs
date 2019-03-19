using AutoMapper;
using Cafe.Core.Auth;
using Cafe.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Cafe.Business.Auth.Handlers
{
    public abstract class BaseAuthHandler<TCommand> : BaseHandler<TCommand>
    {
        public BaseAuthHandler(
            IValidator<TCommand> validator,
            UserManager<User> userManager,
            IJwtFactory jwtFactory,
            IMapper mapper)
            : base (validator)
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
