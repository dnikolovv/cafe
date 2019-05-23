using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Optional;
using Optional.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cafe.Persistance.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Option<Unit, Error>> Register(User user, string password)
        {
            var creationResult = (await _userManager.CreateAsync(user, password))
                .SomeWhen(
                    x => x.Succeeded,
                    x => Error.Validation(x.Errors.Select(e => e.Description)));

            return creationResult
                .Map(_ => Unit.Value);
        }

        public async Task<Option<User>> Get(Guid id) =>
            (await _userManager
                .FindByIdAsync(id.ToString()))
                .SomeNotNull();

        public async Task<Unit> ReplaceClaim(User account, string claimType, string claimValue)
        {
            var claimToReplace = (await _userManager.GetClaimsAsync(account))
                .FirstOrDefault(c => c.Type == claimType);

            var claimToAdd = new Claim(claimType, claimValue);

            if (claimToReplace != null)
            {
                await _userManager.ReplaceClaimAsync(account, claimToReplace, claimToAdd);
            }
            else
            {
                await _userManager.AddClaimAsync(account, claimToAdd);
            }

            return Unit.Value;
        }

        public async Task<Option<User>> GetByEmail(string email) =>
            (await _userManager
                .FindByEmailAsync(email))
                .SomeNotNull();

        public Task<bool> CheckPassword(User user, string password) =>
            _userManager
                .CheckPasswordAsync(user, password);

        public Task<IList<Claim>> GetClaims(User user) =>
            _userManager
                .GetClaimsAsync(user);
    }
}
