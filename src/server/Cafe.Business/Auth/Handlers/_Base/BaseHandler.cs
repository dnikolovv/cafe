using AutoMapper;
using Cafe.Core.Auth;
using Cafe.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Cafe.Business.Auth.Handlers
{
    public abstract class BaseHandler
    {
        public BaseHandler(
            UserManager<User> userManager,
            IJwtFactory jwtFactory,
            IMapper mapper)
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
