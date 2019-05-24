using Cafe.Core;
using Cafe.Core.TableContext.Queries;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.TableContext.QueryHandlers
{
    public class GetAllTablesHandler : IQueryHandler<GetAllTables, IList<TableView>>
    {
        private readonly ITableViewRepository _tableViewRepository;

        public GetAllTablesHandler(ITableViewRepository tableViewRepository)
        {
            _tableViewRepository = tableViewRepository;
        }

        public Task<IList<TableView>> Handle(GetAllTables request, CancellationToken cancellationToken) =>
            _tableViewRepository
                .GetAll();
    }
}
