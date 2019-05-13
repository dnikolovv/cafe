using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Tab
{
    public class OrderMenuItemsResourcePolicy : IPolicy<OrderMenuItemsResource>
    {
        public Action<LinksPolicyBuilder<OrderMenuItemsResource>> PolicyConfiguration => policy =>
        {
            policy.RequireSelfLink();
            policy.RequireRoutedLink(LinkNames.Tab.GetTab, nameof(TabController.GetTabView), x => new { id = x.TabId });
            policy.RequireRoutedLink(LinkNames.Tab.Close, nameof(TabController.CloseTab));
            policy.RequireRoutedLink(LinkNames.Tab.OrderItems, nameof(TabController.OrderMenuItems));
            policy.RequireRoutedLink(LinkNames.Tab.ServeItems, nameof(TabController.ServeMenuItems));
            policy.RequireRoutedLink(LinkNames.Tab.RejectItems, nameof(TabController.RejectMenuItems));
        };
    }
}
