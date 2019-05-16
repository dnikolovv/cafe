using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Order
{
    public class ToGoOrderResourcePolicy : IPolicy<ToGoOrderResource>
    {
        public Action<LinksPolicyBuilder<ToGoOrderResource>> PolicyConfiguration => policy =>
        {
            policy.RequireRoutedLink(LinkNames.Self, nameof(OrderController.GetOrder), x => new { id = x.Id });
            policy.RequireRoutedLink(LinkNames.Order.Complete, nameof(OrderController.CompleteToGoOrder), null, cond => cond.AuthorizeRoute());
            policy.RequireRoutedLink(LinkNames.Order.Confirm, nameof(OrderController.ConfirmToGoOrder), null, cond => cond.AuthorizeRoute());
        };
    }
}
