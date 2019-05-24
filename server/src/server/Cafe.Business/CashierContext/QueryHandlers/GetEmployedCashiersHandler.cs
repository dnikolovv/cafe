using Cafe.Core;
using Cafe.Core.CashierContext.Queries;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.CashierContext.QueryHandlers
{
    public class GetEmployedCashiersHandler : IQueryHandler<GetEmployedCashiers, IList<CashierView>>
    {
        private readonly ICashierViewRepository _cashierViewRepository;

        public GetEmployedCashiersHandler(ICashierViewRepository cashierViewRepository)
        {
            _cashierViewRepository = cashierViewRepository;
        }

        public Task<IList<CashierView>> Handle(GetEmployedCashiers request, CancellationToken cancellationToken) =>
            _cashierViewRepository
                .GetAll();
    }
}
