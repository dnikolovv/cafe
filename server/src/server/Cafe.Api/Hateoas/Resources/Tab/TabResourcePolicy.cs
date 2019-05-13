using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Tab
{
    public class TabResourcePolicy : IPolicy<TabResource>
    {
        public Action<LinksPolicyBuilder<TabResource>> PolicyConfiguration => policy =>
        {
            policy.RequireRoutedLink(LinkNames.Self, nameof(TabController.GetTabView), x => new { id = x.Id });
            policy.RequireRoutedLink(LinkNames.Tab.Close, nameof(TabController.CloseTab), null, cond => cond.Assert(x => x.IsOpen));
            policy.RequireRoutedLink(LinkNames.Tab.OrderItems, nameof(TabController.OrderMenuItems), null, cond => cond.Assert(x => x.IsOpen));
            policy.RequireRoutedLink(LinkNames.Tab.ServeItems, nameof(TabController.ServeMenuItems), null, cond => cond.Assert(x => x.IsOpen && x.OutstandingMenuItems.Count > 0));
            policy.RequireRoutedLink(LinkNames.Tab.RejectItems, nameof(TabController.RejectMenuItems), null, cond => cond.Assert(x => x.IsOpen && x.OutstandingMenuItems.Count > 0));
        };
    }
}
