using Cafe.Domain.Events;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Cafe.Api.Hubs
{
    public class ConfirmedOrdersHub : Hub
    {
        public Task OrderConfirmed(OrderConfirmed notification) =>
            Clients.All.SendAsync(nameof(OrderConfirmed), notification);
    }
}
