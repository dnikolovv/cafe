using Cafe.Core;
using Cafe.Core.CashierContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using Optional;
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

        public async Task<Option<IList<CashierView>, Error>> Handle(GetEmployedCashiers request, CancellationToken cancellationToken)
        {
            var cashiers = await _cashierViewRepository
                .GetAll();

            return cashiers
                .Some<IList<CashierView>, Error>();
        }
    }
}
