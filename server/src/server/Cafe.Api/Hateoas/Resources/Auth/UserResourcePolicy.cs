using Cafe.Api.Controllers;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources.Auth
{
    public class UserResourcePolicy : IPolicy<UserResource>
    {
        public Action<LinksPolicyBuilder<UserResource>> PolicyConfiguration => policy =>
        {
            policy.RequireRoutedLink(LinkNames.Auth.AssignWaiter, nameof(AuthController.AssignWaiterToAccount), null, cond => cond.AuthorizeRoute());
            policy.RequireRoutedLink(LinkNames.Auth.AssignCashier, nameof(AuthController.AssignCashierToAccount), null, cond => cond.AuthorizeRoute());
            policy.RequireRoutedLink(LinkNames.Auth.AssignManager, nameof(AuthController.AssignManagerToAccount), null, cond => cond.AuthorizeRoute());
            policy.RequireRoutedLink(LinkNames.Auth.AssignBarista, nameof(AuthController.AssignBaristaToAccount), null, cond => cond.AuthorizeRoute());
        };
    }
}
