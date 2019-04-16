using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cafe.Core;
using Cafe.Core.OrderContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Views;
using Cafe.Persistance.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Optional;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.OrderContext.QueryHandlers
{
    public class GetOrdersByStatusHandler : IQueryHandler<GetOrdersByStatus, IList<ToGoOrderView>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetOrdersByStatusHandler(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Option<IList<ToGoOrderView>, Error>> Handle(GetOrdersByStatus query, CancellationToken cancellationToken)
        {
            var orders = await _dbContext
                .ToGoOrders
                .Where(o => o.Status == query.Status)
                .Include(o => o.OrderedItems)
                .ProjectTo<ToGoOrderView>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return orders
                .Some<IList<ToGoOrderView>, Error>();
        }
    }
}
