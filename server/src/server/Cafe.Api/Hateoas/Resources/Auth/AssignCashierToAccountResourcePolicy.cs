using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Auth
{

    public class AssignCashierToAccountResourcePolicy : IPolicy<AssignCashierToAccountResource>
    {
        public Action<LinksPolicyBuilder<AssignCashierToAccountResource>> PolicyConfiguration => policy =>
        {
            policy.RequireSelfLink();
            policy.RequireRoutedLink(LinkNames.Auth.AssignBarista, nameof(AuthController.AssignBaristaToAccount));
            policy.RequireRoutedLink(LinkNames.Auth.AssignManager, nameof(AuthController.AssignManagerToAccount));
            policy.RequireRoutedLink(LinkNames.Auth.AssignWaiter, nameof(AuthController.AssignWaiterToAccount));
        };
    }
}
