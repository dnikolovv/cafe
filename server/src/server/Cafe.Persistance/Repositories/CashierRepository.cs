using Cafe.Domain.Entities;
using Cafe.Domain.Repositories;
using Cafe.Persistance.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Optional;
using System;
using System.Threading.Tasks;

namespace Cafe.Persistance.Repositories
{
    public class CashierRepository : ICashierRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CashierRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Add(Cashier cashier)
        {
            _dbContext.Add(cashier);
            await _dbContext.SaveChangesAsync();
            return Unit.Value;
        }

        public async Task<Option<Cashier>> Get(Guid id) =>
            (await _dbContext
                .Cashiers
                .FirstOrDefaultAsync(c => c.Id == id))
            .SomeNotNull();
    }
}
