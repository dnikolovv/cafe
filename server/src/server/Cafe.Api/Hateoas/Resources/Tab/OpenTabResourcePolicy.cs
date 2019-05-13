using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Tab
{
    public class OpenTabResourcePolicy : IPolicy<OpenTabResource>
    {
        public Action<LinksPolicyBuilder<OpenTabResource>> PolicyConfiguration => policy =>
        {
            policy.RequireSelfLink();
            policy.RequireRoutedLink(LinkNames.Tab.GetTab, nameof(TabController.GetTabView), x => new { id = x.Id });
            policy.RequireRoutedLink(LinkNames.Tab.Close, nameof(TabController.CloseTab));
            policy.RequireRoutedLink(LinkNames.Tab.OrderItems, nameof(TabController.OrderMenuItems));
        };
    }
}
