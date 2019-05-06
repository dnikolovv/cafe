using Cafe.Core.AuthContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Cafe.Api.Hubs
{
    [Authorize(Policy = AuthConstants.Policies.IsAdminOrManager)]
    public class HiredWaitersHub : Hub
    {
    }
}
