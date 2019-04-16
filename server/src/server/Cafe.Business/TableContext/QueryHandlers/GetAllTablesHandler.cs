using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cafe.Core;
using Cafe.Core.TableContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Views;
using Cafe.Persistance.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Optional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.TableContext.QueryHandlers
{
    public class GetAllTablesHandler : IQueryHandler<GetAllTables, IList<TableView>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllTablesHandler(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Option<IList<TableView>, Error>> Handle(GetAllTables request, CancellationToken cancellationToken)
        {
            var tables = await _dbContext
                .Tables
                .ProjectTo<TableView>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return tables
                .Some<IList<TableView>, Error>();
        }
    }
}
