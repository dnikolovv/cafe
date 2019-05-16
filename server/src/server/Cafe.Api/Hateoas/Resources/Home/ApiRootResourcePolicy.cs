using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Home
{
    public class ApiRootResourcePolicy : IPolicy<ApiRootResource>
    {
        public Action<LinksPolicyBuilder<ApiRootResource>> PolicyConfiguration => policy =>
        {
            policy.RequireRoutedLink(LinkNames.Auth.Login, nameof(AuthController.Login), null, cond => cond.Assert(x => !x.IsUserLoggedIn));
            policy.RequireRoutedLink(LinkNames.Auth.Register, nameof(AuthController.Register), null, cond => cond.Assert(x => !x.IsUserLoggedIn));
            policy.RequireRoutedLink(LinkNames.Auth.Logout, nameof(AuthController.Logout), null, cond => cond.AuthorizeRoute());
        };
    }
}
