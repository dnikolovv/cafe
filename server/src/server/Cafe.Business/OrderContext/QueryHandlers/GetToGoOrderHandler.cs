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
    public class GetToGoOrderHandler : IQueryHandler<GetToGoOrder, Option<ToGoOrderView, Error>>
    {
        private readonly IToGoOrderViewRepository _toGoOrderRepository;

        public GetToGoOrderHandler(IToGoOrderViewRepository toGoOrderRepository)
        {
            _toGoOrderRepository = toGoOrderRepository;
        }

        public async Task<Option<ToGoOrderView, Error>> Handle(GetToGoOrder request, CancellationToken cancellationToken) =>
            (await _toGoOrderRepository
                .Get(request.Id))
                .WithException(Error.NotFound($"Order {request.Id} was not found."));
    }
}
