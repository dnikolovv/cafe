using AutoMapper;
using Cafe.Core;
using Cafe.Core.OrderContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Views;
using Cafe.Persistance.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Optional;
using Optional.Async;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.OrderContext.QueryHandlers
{
    public class GetToGoOrderHandler : IQueryHandler<GetToGoOrder, ToGoOrderView>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetToGoOrderHandler(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Option<ToGoOrderView, Error>> Handle(GetToGoOrder request, CancellationToken cancellationToken) =>
            (await GetOrderIfExists(request.Id, cancellationToken))
                .Map(_mapper.Map<ToGoOrderView>);

        private async Task<Option<ToGoOrder, Error>> GetOrderIfExists(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _dbContext
                .ToGoOrders
                .Include(o => o.OrderedItems)
                .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

            return order
                .SomeNotNull(Error.NotFound($"Order {orderId} was not found."));
        }
    }
}
