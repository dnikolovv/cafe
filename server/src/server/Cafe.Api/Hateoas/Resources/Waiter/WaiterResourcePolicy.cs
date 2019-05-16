using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Waiter
{
    public class WaiterResourcePolicy : IPolicy<WaiterResource>
    {
        public Action<LinksPolicyBuilder<WaiterResource>> PolicyConfiguration => policy =>
        {
            policy.RequireSelfLink();
            policy.RequireRoutedLink(LinkNames.Waiter.GetEmployedWaiters, nameof(WaiterController.GetEmployedWaiters));
            policy.RequireRoutedLink(LinkNames.Waiter.AssignTable, nameof(WaiterController.AssignTable));
        };
    }
}
