using Cafe.Core;
using Cafe.Core.AuthContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Views;
using Optional;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.AuthContext.QueryHandlers
{
    public class GetUserHandler : IQueryHandler<GetUser, UserView>
    {
        private readonly IUsersService _usersService;

        public GetUserHandler(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public Task<Option<UserView, Error>> Handle(GetUser request, CancellationToken cancellationToken) =>
            _usersService.GetUser(request.Id);
    }
}
