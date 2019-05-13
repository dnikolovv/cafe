using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Tab
{
    public class TabsContainerResourcePolicy : IPolicy<TabsContainerResource>
    {
        public Action<LinksPolicyBuilder<TabsContainerResource>> PolicyConfiguration => policy =>
        {
            policy.RequireRoutedLink("self", nameof(TabController.GetAllOpenTabs));
        };
    }
}
