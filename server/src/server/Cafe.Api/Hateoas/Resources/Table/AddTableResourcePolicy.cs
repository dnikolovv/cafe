using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Table
{
    public class AddTableResourcePolicy : IPolicy<AddTableResource>
    {
        public Action<LinksPolicyBuilder<AddTableResource>> PolicyConfiguration => policy =>
        {
            policy.RequireSelfLink();
            policy.RequireRoutedLink(LinkNames.Table.GetAll, nameof(TableController.GetAllTables));
        };
    }
}
