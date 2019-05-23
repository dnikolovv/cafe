using Cafe.Core;
using Cafe.Core.AuthContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using Optional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.AuthContext.QueryHandlers
{
    public class GetAllUserAccountsHandler : IQueryHandler<GetAllUserAccounts, IList<UserView>>
    {
        private readonly IUserViewRepository _userViewRepository;

        public GetAllUserAccountsHandler(IUserViewRepository userViewRepository)
        {
            _userViewRepository = userViewRepository;
        }

        public async Task<Option<IList<UserView>, Error>> Handle(GetAllUserAccounts request, CancellationToken cancellationToken) =>
            (await _userViewRepository.GetAll())
                .Some<IList<UserView>, Error>();
    }
}