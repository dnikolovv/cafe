using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Table
{
    public class CallWaiterResourcePolicy : IPolicy<CallWaiterResource>
    {
        public Action<LinksPolicyBuilder<CallWaiterResource>> PolicyConfiguration => policy =>
        {
            policy.RequireSelfLink();
            policy.RequireRoutedLink(LinkNames.Table.RequestBill, nameof(TableController.RequestBill), x => new { tableNumber = x.TableNumber });
            policy.RequireRoutedLink(LinkNames.Table.GetAll, nameof(TableController.GetAllTables));
        };
    }
}
