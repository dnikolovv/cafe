using Cafe.Core.AuthContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Cafe.Api.Hubs
{
    [Authorize(Policy = AuthConstants.Policies.IsAdminOrWaiter)]
    public class TableActionsHub : Hub
    {
    }
}
