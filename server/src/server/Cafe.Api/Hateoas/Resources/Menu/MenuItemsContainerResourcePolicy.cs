using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Menu
{
    public class MenuItemsContainerResourcePolicy : IPolicy<MenuItemsContainerResource>
    {
        public Action<LinksPolicyBuilder<MenuItemsContainerResource>> PolicyConfiguration => policy =>
        {
            policy.RequireSelfLink();
        };
    }
}
