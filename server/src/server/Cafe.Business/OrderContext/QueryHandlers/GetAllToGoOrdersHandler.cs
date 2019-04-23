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
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.OrderContext.QueryHandlers
{
    public class GetAllToGoOrdersHandler : IQueryHandler<GetAllToGoOrders, IList<ToGoOrderView>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllToGoOrdersHandler(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Option<IList<ToGoOrderView>, Error>> Handle(GetAllToGoOrders request, CancellationToken cancellationToken)
        {
            var orders = await _dbContext
                .ToGoOrders
                .Include(o => o.OrderedItems)
                .ProjectTo<ToGoOrderView>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return orders
                .Some<IList<ToGoOrderView>, Error>();
        }
    }
}
