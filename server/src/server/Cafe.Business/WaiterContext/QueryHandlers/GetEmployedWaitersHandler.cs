using Cafe.Core;
using Cafe.Core.WaiterContext.Queries;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.WaiterContext.QueryHandlers
{
    public class GetEmployedWaitersHandler : IQueryHandler<GetEmployedWaiters, IList<WaiterView>>
    {
        private readonly IWaiterViewRepository _waiterViewRepository;

        public GetEmployedWaitersHandler(IWaiterViewRepository waiterViewRepository)
        {
            _waiterViewRepository = waiterViewRepository;
        }

        public Task<IList<WaiterView>> Handle(GetEmployedWaiters request, CancellationToken cancellationToken) =>
            _waiterViewRepository
                .GetAll();
    }
}
