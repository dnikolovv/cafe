using Cafe.Api.Controllers;
using Cafe.Api.Resources.Tab;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas
{
    public class OrderMenuItemsResourcePolicy : IPolicy<OrderMenuItemsResource>
    {
        public Action<LinksPolicyBuilder<OrderMenuItemsResource>> PolicyConfiguration => policy =>
        {
            policy.RequireRoutedLink("self", nameof(TabController.OrderMenuItems));
            policy.RequireRoutedLink("view", nameof(TabController.GetTabView), x => new { id = x.TabId });
            policy.RequireRoutedLink("serve-items", nameof(TabController.ServeMenuItems));
            policy.RequireRoutedLink("reject-items", nameof(TabController.RejectMenuItems));
            policy.RequireRoutedLink("close", nameof(TabController.CloseTab));
        };
    }
}
