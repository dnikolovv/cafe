using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Auth
{

    public class AssignBaristaToAccountResourcePolicy : IPolicy<AssignBaristaToAccountResource>
    {
        public Action<LinksPolicyBuilder<AssignBaristaToAccountResource>> PolicyConfiguration => policy =>
        {
            policy.RequireSelfLink();
            policy.RequireRoutedLink(LinkNames.Auth.AssignCashier, nameof(AuthController.AssignCashierToAccount));
            policy.RequireRoutedLink(LinkNames.Auth.AssignManager, nameof(AuthController.AssignManagerToAccount));
            policy.RequireRoutedLink(LinkNames.Auth.AssignWaiter, nameof(AuthController.AssignWaiterToAccount));
        };
    }
}
