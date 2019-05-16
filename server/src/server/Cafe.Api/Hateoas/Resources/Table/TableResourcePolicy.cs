using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Table
{
    public class TableResourcePolicy : IPolicy<TableResource>
    {
        public Action<LinksPolicyBuilder<TableResource>> PolicyConfiguration => policy =>
        {
            policy.RequireRoutedLink(LinkNames.Table.CallWaiter, nameof(TableController.CallWaiter), x => new { tableNumber = x.Number }, cond => cond.Assert(x => x.WaiterId != default));
            policy.RequireRoutedLink(LinkNames.Table.RequestBill, nameof(TableController.RequestBill), x => new { tableNumber = x.Number }, cond => cond.Assert(x => x.WaiterId != default));
            policy.RequireRoutedLink(LinkNames.Table.GetAll, nameof(TableController.GetAllTables));
        };
    }
}
