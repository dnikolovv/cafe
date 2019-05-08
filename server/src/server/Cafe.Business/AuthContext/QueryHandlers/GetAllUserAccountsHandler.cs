using Cafe.Core;
using Cafe.Core.AuthContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Views;
using Optional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.AuthContext.QueryHandlers
{
    public class GetAllUserAccountsHandler : IQueryHandler<GetAllUserAccounts, IList<UserView>>
    {
        private readonly IUsersService _usersService;

        public GetAllUserAccountsHandler(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task<Option<IList<UserView>, Error>> Handle(GetAllUserAccounts request, CancellationToken cancellationToken) =>
            (await _usersService.GetAllUsers())
                .Some<IList<UserView>, Error>();
    }
}