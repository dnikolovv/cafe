using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Order
{
    public class OrderToGoResourcePolicy : IPolicy<OrderToGoResource>
    {
        public Action<LinksPolicyBuilder<OrderToGoResource>> PolicyConfiguration => policy =>
        {
            policy.RequireSelfLink();
            policy.RequireRoutedLink(LinkNames.Order.Get, nameof(OrderController.GetOrder), x => new { id = x.OrderId });
            policy.RequireRoutedLink(LinkNames.Order.GetAll, nameof(OrderController.GetAllOrders));
            policy.RequireRoutedLink(LinkNames.Order.Confirm, nameof(OrderController.ConfirmToGoOrder));
        };
    }
}
