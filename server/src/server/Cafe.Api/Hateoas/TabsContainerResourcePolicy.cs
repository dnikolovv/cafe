using Cafe.Api.Controllers;
using Cafe.Api.Resources.Tab;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas
{
    public class TabsContainerResourcePolicy : IPolicy<TabsContainerResource>
    {
        public Action<LinksPolicyBuilder<TabsContainerResource>> PolicyConfiguration => policy =>
        {
            policy.RequireRoutedLink("self", nameof(TabController.GetAllOpenTabs));
        };
    }
}
