using Cafe.Api.Controllers;
using Cafe.Api.Resources.Tab;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas
{
    public class OpenTabResourcePolicy : IPolicy<OpenTabResource>
    {
        public Action<LinksPolicyBuilder<OpenTabResource>> PolicyConfiguration => policy =>
        {
            policy.RequireRoutedLink("view", nameof(TabController.GetTabView), x => new { id = x.Id });
            policy.RequireRoutedLink("close", nameof(TabController.CloseTab));
            policy.RequireRoutedLink("order-items", nameof(TabController.OrderMenuItems));
        };
    }
}
