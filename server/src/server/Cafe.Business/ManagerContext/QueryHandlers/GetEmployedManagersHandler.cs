using Cafe.Core;
using Cafe.Core.ManagerContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using Optional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.ManagerContext.QueryHandlers
{
    public class GetEmployedManagersHandler : IQueryHandler<GetEmployedManagers, IList<ManagerView>>
    {
        private readonly IManagerViewRepository _managerViewRepository;

        public GetEmployedManagersHandler(IManagerViewRepository managerViewRepository)
        {
            _managerViewRepository = managerViewRepository;
        }

        public async Task<Option<IList<ManagerView>, Error>> Handle(GetEmployedManagers request, CancellationToken cancellationToken)
        {
            var managers = await _managerViewRepository
                .GetAll();

            return managers
                .Some<IList<ManagerView>, Error>();
        }
    }
}
