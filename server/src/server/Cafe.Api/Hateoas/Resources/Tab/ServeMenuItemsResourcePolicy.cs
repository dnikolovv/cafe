using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Tab
{
    public class ServeMenuItemsResourcePolicy : IPolicy<ServeMenuItemsResource>
    {
        public Action<LinksPolicyBuilder<ServeMenuItemsResource>> PolicyConfiguration => policy =>
        {
            policy.RequireRoutedLink("self", nameof(TabController.ServeMenuItems));
            policy.RequireRoutedLink("view", nameof(TabController.GetTabView), x => new { id = x.TabId });
            policy.RequireRoutedLink("close", nameof(TabController.CloseTab));
            policy.RequireRoutedLink("order-items", nameof(TabController.OrderMenuItems));

            policy.RequireSelfLink();
            policy.RequireRoutedLink(LinkNames.Tab.GetTab, nameof(TabController.GetTabView), x => new { id = x.TabId });
            policy.RequireRoutedLink(LinkNames.Tab.Close, nameof(TabController.CloseTab));
            policy.RequireRoutedLink(LinkNames.Tab.OrderItems, nameof(TabController.OrderMenuItems));
        };
    }
}
