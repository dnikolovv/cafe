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
        private readonly IHubContext<TableActionsHub> _hubContext;
        private readonly ApplicationDbContext _dbContext;

        public TableActionsDispatcher(IHubContext<TableActionsHub> hubContext, ApplicationDbContext dbContext)
        {
            _hubContext = hubContext;
            _dbContext = dbContext;
        }

        public async Task Handle(WaiterCalled notification, CancellationToken cancellationToken)
        {
            var accountIdsToNotify = (await _dbContext
                .UserClaims
                .Where(c => c.ClaimType == AuthConstants.ClaimTypes.WaiterId)
                .Select(uc => uc.UserId)
                .ToArrayAsync())
                .Select(g => g.ToString())
                .ToArray();

            await _hubContext
                .Clients
                .Users(accountIdsToNotify)
                .SendAsync(nameof(WaiterCalled), notification);
        }

        public Task Handle(BillRequested notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
