using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Order
{
    public class ToGoOrderContainerResourcePolicy : IPolicy<ToGoOrderContainerResource>
    {
        public Action<LinksPolicyBuilder<ToGoOrderContainerResource>> PolicyConfiguration => policy =>
        {
            policy.RequireSelfLink();
            policy.RequireRoutedLink(LinkNames.Order.New, nameof(OrderController.OrderToGo));
        };
    }
}
