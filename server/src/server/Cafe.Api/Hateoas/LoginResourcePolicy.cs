using Cafe.Api.Controllers;
using Cafe.Api.Resources.Auth;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas
{
    public class LoginResourcePolicy : IPolicy<LoginResource>
    {
        public Action<LinksPolicyBuilder<LoginResource>> PolicyConfiguration => policy =>
        {
            policy.RequireSelfLink();
            policy.RequireRoutedLink("get-current-user", nameof(AuthController.GetCurrentUser));
            policy.RequireRoutedLink("logout", nameof(AuthController.Logout));
        };
    }
}
