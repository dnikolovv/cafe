using Cafe.Api.Controllers;
using Cafe.Api.Resources.Tab;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas
{
    public class ServeMenuItemsResourcePolicy : IPolicy<ServeMenuItemsResource>
    {
        public Action<LinksPolicyBuilder<ServeMenuItemsResource>> PolicyConfiguration => policy =>
        {
            policy.RequireRoutedLink("self", nameof(TabController.ServeMenuItems));
            policy.RequireRoutedLink("view", nameof(TabController.GetTabView), x => new { id = x.TabId });
            policy.RequireRoutedLink("close", nameof(TabController.CloseTab));
            policy.RequireRoutedLink("order-items", nameof(TabController.OrderMenuItems));
        };
    }
}
