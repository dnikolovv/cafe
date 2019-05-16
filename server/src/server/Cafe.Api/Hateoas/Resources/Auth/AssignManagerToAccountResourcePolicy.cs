using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Auth
{
    public class AssignManagerToAccountResourcePolicy : IPolicy<AssignManagerToAccountResource>
    {
        public Action<LinksPolicyBuilder<AssignManagerToAccountResource>> PolicyConfiguration => policy =>
        {
            policy.RequireSelfLink();
            policy.RequireRoutedLink(LinkNames.Auth.AssignBarista, nameof(AuthController.AssignBaristaToAccount));
            policy.RequireRoutedLink(LinkNames.Auth.AssignCashier, nameof(AuthController.AssignCashierToAccount));
            policy.RequireRoutedLink(LinkNames.Auth.AssignWaiter, nameof(AuthController.AssignWaiterToAccount));
        };
    }
}