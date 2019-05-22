using Cafe.Core;
using Cafe.Core.OrderContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using Optional;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.OrderContext.QueryHandlers
{
    public class GetToGoOrderHandler : IQueryHandler<GetToGoOrder, ToGoOrderView>
    {
        private readonly IToGoOrderViewRepository _toGoOrderRepository;

        public GetToGoOrderHandler(IToGoOrderViewRepository toGoOrderRepository)
        {
            _toGoOrderRepository = toGoOrderRepository;
        }

        public async Task<Option<ToGoOrderView, Error>> Handle(GetToGoOrder request, CancellationToken cancellationToken)
        {
            var order = await _toGoOrderRepository
                .Get(request.Id);

            return order
                .WithException(Error.NotFound($"Order {request.Id} was not found."));
        }
    }
}
