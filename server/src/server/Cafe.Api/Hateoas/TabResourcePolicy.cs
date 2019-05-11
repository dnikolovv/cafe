using Cafe.Api.Controllers;
using Cafe.Api.Resources.Tab;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas
{
    public class TabResourcePolicy : IPolicy<TabResource>
    {
        public Action<LinksPolicyBuilder<TabResource>> PolicyConfiguration => policy =>
        {
            policy.RequireRoutedLink("self", nameof(TabController.GetTabView), x => new { id = x.Id });
            policy.RequireRoutedLink("close", nameof(TabController.CloseTab), null, cond => cond.Assert(x => x.IsOpen));
            policy.RequireRoutedLink("order-items", nameof(TabController.OrderMenuItems), null, cond => cond.Assert(x => x.IsOpen));
            policy.RequireRoutedLink("serve-items", nameof(TabController.ServeMenuItems), null, cond => cond.Assert(x => x.IsOpen && x.OutstandingMenuItems.Count > 0));
            policy.RequireRoutedLink("reject-items", nameof(TabController.RejectMenuItems), null, cond => cond.Assert(x => x.IsOpen && x.OutstandingMenuItems.Count > 0));
        };
    }
}
