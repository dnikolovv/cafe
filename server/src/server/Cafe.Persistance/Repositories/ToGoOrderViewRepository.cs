using AutoMapper;
using Cafe.Domain.Entities;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using Cafe.Persistance.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Optional;
using Optional.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cafe.Persistance.Repositories
{
    public class ToGoOrderViewRepository : IToGoOrderViewRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ToGoOrderViewRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Option<ToGoOrderView>> Get(Guid id) =>
            (await _dbContext
                .ToGoOrders
                .FirstOrDefaultAsync(o => o.Id == id))
                .SomeNotNull()
                .Map(_mapper.Map<ToGoOrderView>);

        public Task<IList<ToGoOrderView>> GetAll() =>
            FetchOrders();

        public Task<IList<ToGoOrderView>> GetByStatus(ToGoOrderStatus status) =>
            FetchOrders(o => o.Status == status);

        private async Task<IList<ToGoOrderView>> FetchOrders(Expression<Func<ToGoOrder, bool>> predicate = null) =>
            (await _dbContext
                    .ToGoOrders
                    .Include(o => o.OrderedItems)
                        .ThenInclude(i => i.MenuItem)
                    .Where(predicate ?? (_ => true))
                    .ToListAsync())

                    // Not using ProjectTo because we've registered a custom converter
                    .Select(_mapper.Map<ToGoOrderView>)
                    .ToList();
    }
}
