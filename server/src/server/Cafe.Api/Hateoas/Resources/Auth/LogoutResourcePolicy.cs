using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Auth
{
    public class LogoutResourcePolicy : IPolicy<LogoutResource>
    {
        public Action<LinksPolicyBuilder<LogoutResource>> PolicyConfiguration => policy =>
        {
            policy.RequireRoutedLink(LinkNames.Auth.Login, nameof(AuthController.Login));
            policy.RequireRoutedLink(LinkNames.Auth.Register, nameof(AuthController.Register));
        };
    }
}
