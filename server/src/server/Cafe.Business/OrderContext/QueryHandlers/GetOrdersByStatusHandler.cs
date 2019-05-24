using Cafe.Core;
using Cafe.Core.OrderContext.Queries;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.OrderContext.QueryHandlers
{
    public class GetOrdersByStatusHandler : IQueryHandler<GetOrdersByStatus, IList<ToGoOrderView>>
    {
        private readonly IToGoOrderViewRepository _toGoOrderViewRepository;

        public GetOrdersByStatusHandler(IToGoOrderViewRepository toGoOrderViewRepository)
        {
            _toGoOrderViewRepository = toGoOrderViewRepository;
        }

        public Task<IList<ToGoOrderView>> Handle(GetOrdersByStatus query, CancellationToken cancellationToken) =>
            _toGoOrderViewRepository
                .GetByStatus(query.Status);
    }
}
