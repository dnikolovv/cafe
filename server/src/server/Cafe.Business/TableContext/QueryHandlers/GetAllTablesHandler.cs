using Cafe.Core;
using Cafe.Core.TableContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using Optional;
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

        public async Task<Option<IList<TableView>, Error>> Handle(GetAllTables request, CancellationToken cancellationToken)
        {
            var tables = await _tableViewRepository
                .GetAll();

            return tables
                .Some<IList<TableView>, Error>();
        }
    }
}
