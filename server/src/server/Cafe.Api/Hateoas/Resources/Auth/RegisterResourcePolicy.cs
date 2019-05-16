using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Auth
{
    public class RegisterResourcePolicy : IPolicy<RegisterResource>
    {
        public Action<LinksPolicyBuilder<RegisterResource>> PolicyConfiguration => policy =>
        {
            policy.RequireSelfLink();
            policy.RequireRoutedLink(LinkNames.Auth.Login, nameof(AuthController.Login));
        };
    }
}
