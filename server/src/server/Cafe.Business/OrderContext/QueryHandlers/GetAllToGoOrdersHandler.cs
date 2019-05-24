using Cafe.Core;
using Cafe.Core.OrderContext.Queries;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
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

        public Task<IList<ToGoOrderView>> Handle(GetAllToGoOrders request, CancellationToken cancellationToken) =>
            _toGoOrderViewRepository
                .GetAll();
    }
}
