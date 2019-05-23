using Cafe.Domain.Entities;
using Cafe.Domain.Repositories;
using Cafe.Persistance.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Optional;
using Optional.Async;
using System;
using System.Threading.Tasks;

namespace Cafe.Persistance.Repositories
{
    public class ToGoOrderRepository : IToGoOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ToGoOrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Add(ToGoOrder order)
        {
            _dbContext.ToGoOrders.Add(order);
            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }

        public Task<Option<ToGoOrder>> Get(Guid id) =>
            _dbContext
                .ToGoOrders
                .Include(o => o.OrderedItems)
                    .ThenInclude(i => i.MenuItem)
                .FirstOrDefaultAsync(o => o.Id == id)
                .SomeNotNull();

        public async Task<Unit> Update(ToGoOrder order)
        {
            _dbContext.ToGoOrders.Update(order);
            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
