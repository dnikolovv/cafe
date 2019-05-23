using Cafe.Domain.Entities;
using Cafe.Domain.Repositories;
using Cafe.Persistance.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cafe.Persistance.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MenuItemRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Add(params MenuItem[] items)
        {
            _dbContext.MenuItems.AddRange(items);
            await _dbContext.SaveChangesAsync();
            return Unit.Value;
        }

        public async Task<IList<MenuItem>> GetAll(Expression<Func<MenuItem, bool>> predicate = null) =>
            await _dbContext
                .MenuItems
                .Where(predicate ?? (_ => true))
                .ToListAsync();
    }
}
