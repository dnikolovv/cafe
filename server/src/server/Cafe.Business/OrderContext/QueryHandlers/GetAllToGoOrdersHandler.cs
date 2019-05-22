using Cafe.Core;
using Cafe.Core.OrderContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using Optional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.OrderContext.QueryHandlers
{
    public class GetAllToGoOrdersHandler : IQueryHandler<GetAllToGoOrders, IList<ToGoOrderView>>
    {
        private readonly IToGoOrderViewRepository _toGoOrderViewRepository;

        public GetAllToGoOrdersHandler(IToGoOrderViewRepository toGoOrderViewRepository)
        {
            _toGoOrderViewRepository = toGoOrderViewRepository;
        }

        public async Task<Option<IList<ToGoOrderView>, Error>> Handle(GetAllToGoOrders request, CancellationToken cancellationToken)
        {
            var orders = await _toGoOrderViewRepository
                .GetAll();

            return orders
                .Some<IList<ToGoOrderView>, Error>();
        }
    }
}
