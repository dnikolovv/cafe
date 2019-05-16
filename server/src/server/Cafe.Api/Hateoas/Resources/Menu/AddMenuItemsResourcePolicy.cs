using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Menu
{
    public class AddMenuItemsResourcePolicy : IPolicy<AddMenuItemsResource>
    {
        public Action<LinksPolicyBuilder<AddMenuItemsResource>> PolicyConfiguration => policy =>
        {
            policy.RequireSelfLink();
            policy.RequireRoutedLink(LinkNames.Menu.GetAllMenuItems, nameof(MenuController.GetMenuItems));
        };
    }
}
