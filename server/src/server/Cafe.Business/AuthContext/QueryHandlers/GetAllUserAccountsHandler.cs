using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cafe.Core;
using Cafe.Core.AuthContext;
using Cafe.Core.AuthContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Views;
using Cafe.Persistance.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Optional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.AuthContext.QueryHandlers
{
    public class GetAllUserAccountsHandler : IQueryHandler<GetAllUserAccounts, IList<UserView>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllUserAccountsHandler(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Option<IList<UserView>, Error>> Handle(GetAllUserAccounts request, CancellationToken cancellationToken)
        {
            // This function can be made much simpler by having the User entity include its claims
            // but I'm still not sure whether the User domain entity should include them as they are
            // an implementation detail
            var userClaimsMapping = await _dbContext
                .UserClaims
                .GroupBy(c => c.UserId)
                .Select(g => new { g.Key, Claims = g.Select(c => c) })
                .ToDictionaryAsync(g => g.Key, g => g.Claims);

            var test = await _dbContext.Users.ToListAsync();

            var userViews = (await _dbContext
                .Users
                .ProjectTo<UserView>(_mapper.ConfigurationProvider)
                .ToListAsync())
                .Select(userView =>
                {
                    // TODO: This definitely seems like a bad pattern
                    userView.ManagerId = TryGetIdClaim(userView.Id, userClaimsMapping, c => c.ClaimType == AuthConstants.ClaimTypes.ManagerId);
                    userView.WaiterId = TryGetIdClaim(userView.Id, userClaimsMapping, c => c.ClaimType == AuthConstants.ClaimTypes.WaiterId);
                    userView.CashierId = TryGetIdClaim(userView.Id, userClaimsMapping, c => c.ClaimType == AuthConstants.ClaimTypes.CashierId);
                    userView.BaristaId = TryGetIdClaim(userView.Id, userClaimsMapping, c => c.ClaimType == AuthConstants.ClaimTypes.BaristaId);

                    return userView;
                })
                .ToList();

            return userViews
                .Some<IList<UserView>, Error>();
        }

        private static Guid? TryGetIdClaim(Guid userId, Dictionary<Guid, IEnumerable<IdentityUserClaim<Guid>>> userClaimsMapping, Func<IdentityUserClaim<Guid>, bool> claimPredicate)
        {
            if (!userClaimsMapping.ContainsKey(userId))
                return null;

            var claimValue = userClaimsMapping[userId]
                .FirstOrDefault(claimPredicate)?
                .ClaimValue;

            return claimValue != null ?

                // Purposefully using Parse rather than TryParse because calling this function with an invalid Guid is truly exceptional
                Guid.Parse(claimValue) :
                (Guid?)null;
        }
    }
}