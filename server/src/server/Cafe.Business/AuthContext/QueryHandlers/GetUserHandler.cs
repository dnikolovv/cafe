using Cafe.Core;
using Cafe.Core.AuthContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using Optional;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.AuthContext.QueryHandlers
{
    public class GetUserHandler : IQueryHandler<GetUser, UserView>
    {
        private readonly IUserViewRepository _userViewRepository;

        public GetUserHandler(IUserViewRepository userViewRepository)
        {
            _userViewRepository = userViewRepository;
        }

        public async Task<Option<UserView, Error>> Handle(GetUser request, CancellationToken cancellationToken) =>
            (await _userViewRepository.Get(request.Id))
                .WithException(Error.NotFound($"No user with id {request.Id} was found."));
    }
}
