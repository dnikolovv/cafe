using Cafe.Core;
using Cafe.Core.ManagerContext.Queries;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
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

        public Task<IList<ManagerView>> Handle(GetEmployedManagers request, CancellationToken cancellationToken) =>
            _managerViewRepository
                .GetAll();
    }
}
