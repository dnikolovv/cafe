using Cafe.Core;
using Cafe.Core.WaiterContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using Optional;
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

        public async Task<Option<IList<WaiterView>, Error>> Handle(GetEmployedWaiters request, CancellationToken cancellationToken)
        {
            var waiters = await _waiterViewRepository
                .GetAll();

            return waiters
                .Some<IList<WaiterView>, Error>();
        }
    }
}
