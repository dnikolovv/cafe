using Cafe.Api.Controllers;
using Cafe.Api.Resources.Auth;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas
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
