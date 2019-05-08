using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cafe.Core;
using Cafe.Core.AuthContext;
using Cafe.Domain;
using Cafe.Domain.Views;
using Cafe.Persistance.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Optional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cafe.Business
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UsersService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IList<UserView>> GetAllUsers()
        {
            // This function can be made much simpler by having the User entity include its claims
            // but I'm still not sure whether the User domain entity should include them as they are
            // an implementation detail
            var userClaimsMapping = await _dbContext
                .UserClaims
                .GroupBy(c => c.UserId)
                .Select(g => new { g.Key, Claims = g.Select(c => c) })
                .ToDictionaryAsync(g => g.Key, g => g.Claims);

            var userViews = (await _dbContext
                .Users
                .ProjectTo<UserView>(_mapper.ConfigurationProvider)
                .ToListAsync())
                .Select(userView =>
                    userClaimsMapping.ContainsKey(userView.Id) ?
                        MapRoleIds(userView, userClaimsMapping[userView.Id]) :
                        userView)
                .ToList();

            return userViews;
        }

        public async Task<Option<UserView, Error>> GetUser(Guid id)
        {
            var user = await _dbContext
                .Users
                .FirstOrDefaultAsync(u => u.Id == id);

            var userClaims = await _dbContext
                .UserClaims
                .Where(uc => uc.UserId == id)
                .ToListAsync();

            return user
                .SomeNotNull(Error.NotFound($"No user with id {id} was found."))
                .Map(u => MapRoleIds(_mapper.Map<UserView>(u), userClaims));
        }

        private static UserView MapRoleIds(UserView user, IEnumerable<IdentityUserClaim<Guid>> userClaims)
        {
            if (!userClaims.Any())
                return user;

            // TODO: This definitely seems like a bad pattern
            user.ManagerId = TryGetIdClaim(userClaims, c => c.ClaimType == AuthConstants.ClaimTypes.ManagerId);
            user.WaiterId = TryGetIdClaim(userClaims, c => c.ClaimType == AuthConstants.ClaimTypes.WaiterId);
            user.CashierId = TryGetIdClaim(userClaims, c => c.ClaimType == AuthConstants.ClaimTypes.CashierId);
            user.BaristaId = TryGetIdClaim(userClaims, c => c.ClaimType == AuthConstants.ClaimTypes.BaristaId);

            return user;
        }

        private static Guid? TryGetIdClaim(IEnumerable<IdentityUserClaim<Guid>> claims, Func<IdentityUserClaim<Guid>, bool> claimPredicate)
        {
            var claimValue = claims
                .FirstOrDefault(claimPredicate)?
                .ClaimValue;

            return claimValue != null ?

                // Purposefully using Parse rather than TryParse because calling this function with an invalid Guid is truly exceptional
                Guid.Parse(claimValue) :
                (Guid?)null;
        }
    }
}