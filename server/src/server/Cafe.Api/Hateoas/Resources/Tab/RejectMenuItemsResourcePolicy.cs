using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Tab
{
    public class RejectMenuItemsResourcePolicy : IPolicy<RejectMenuItemsResource>
    {
        public Action<LinksPolicyBuilder<RejectMenuItemsResource>> PolicyConfiguration => policy =>
        {
            policy.RequireRoutedLink("self", nameof(TabController.RejectMenuItems));
            policy.RequireRoutedLink("view", nameof(TabController.GetTabView), x => new { id = x.TabId });
            policy.RequireRoutedLink("order-items", nameof(TabController.OrderMenuItems));
            policy.RequireRoutedLink("serve-items", nameof(TabController.ServeMenuItems));
            policy.RequireRoutedLink("close", nameof(TabController.CloseTab));
        };
    }
}
