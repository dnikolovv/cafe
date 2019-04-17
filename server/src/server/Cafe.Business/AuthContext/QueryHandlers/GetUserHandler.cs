using AutoMapper;
using Cafe.Core;
using Cafe.Core.AuthContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Views;
using Cafe.Persistance.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Optional;
using Optional.Async;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.AuthContext.QueryHandlers
{
    public class GetUserHandler : IQueryHandler<GetUser, UserView>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetUserHandler(IMapper mapper, ApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public Task<Option<UserView, Error>> Handle(GetUser request, CancellationToken cancellationToken) =>
            GetUser(request.Id).MapAsync(async u =>
            _mapper.Map<UserView>(u));

        private async Task<Option<User, Error>> GetUser(Guid id)
        {
            var user = await _dbContext
                .Users
                .FirstOrDefaultAsync(u => u.Id == id);

            return user
                .SomeNotNull(Error.NotFound($"No user with an id of {id} was found."));
        }
    }
}
