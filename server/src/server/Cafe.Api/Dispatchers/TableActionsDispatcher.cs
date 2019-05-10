using Cafe.Api.Hubs;
using Cafe.Core.AuthContext;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Api.Dispatchers
{
    public class TableActionsDispatcher :
        INotificationHandler<WaiterCalled>,
        INotificationHandler<BillRequested>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHubContext<TableActionsHub> _hubContext;

        public TableActionsDispatcher(IHubContext<TableActionsHub> hubContext, ApplicationDbContext dbContext)
        {
            _hubContext = hubContext;
            _dbContext = dbContext;
        }

        public Task Handle(WaiterCalled notification, CancellationToken cancellationToken) =>
            NotifyWaiterAtTable(notification.TableNumber, notification);

        public Task Handle(BillRequested notification, CancellationToken cancellationToken) =>
            NotifyWaiterAtTable(notification.TableNumber, notification);

        private async Task<string[]> GetWaiterAccountsToNotify(int tableNumber)
        {
            var table = await _dbContext
                .Tables
                .FirstOrDefaultAsync(t => t.Number == tableNumber);

            if (table != null && table.WaiterId.HasValue)
            {
                var waiterId = table.WaiterId.ToString();

                var accountsToNotify = (await _dbContext
                    .UserClaims
                    .Where(c => c.ClaimType == AuthConstants.ClaimTypes.WaiterId && c.ClaimValue == waiterId)
                    .Select(uc => uc.UserId)
                    .ToArrayAsync())
                    .Select(g => g.ToString())
                    .ToArray();

                return accountsToNotify;
            }

            return new string[] { };
        }

        private async Task NotifyWaiterAtTable<TNotification>(int tableNumber, TNotification notification)
        {
            var accountIdsToNotify = await GetWaiterAccountsToNotify(tableNumber);

            await _hubContext
                .Clients
                .Users(accountIdsToNotify)
                .SendAsync(typeof(TNotification).Name, notification);
        }
    }
}