using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Tab
{
    public class CloseTabResourcePolicy : IPolicy<CloseTabResource>
    {
        public Action<LinksPolicyBuilder<CloseTabResource>> PolicyConfiguration => policy =>
        {
            policy.RequireSelfLink();
            policy.RequireRoutedLink(LinkNames.Tab.GetTab, nameof(TabController.GetTabView), x => new { id = x.TabId });
            policy.RequireRoutedLink(LinkNames.Tab.Open, nameof(TabController.OpenTab));
        };
    }
}
