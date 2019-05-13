using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Auth
{
    public class UserResourcePolicy : IPolicy<UserResource>
    {
        public Action<LinksPolicyBuilder<UserResource>> PolicyConfiguration => policy =>
        {
            policy.RequireRoutedLink("self", nameof(AuthController.GetCurrentUser));
            policy.RequireRoutedLink("logout", nameof(AuthController.Logout));
        };
    }
}
