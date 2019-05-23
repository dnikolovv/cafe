using Cafe.Domain.Entities;
using MediatR;
using Optional;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cafe.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<Option<User>> Get(Guid id);

        Task<Option<User>> GetByEmail(string email);

        Task<Unit> ReplaceClaim(User account, string claimType, string claimValue);

        Task<Option<Unit, Error>> Register(User user, string password);

        Task<bool> CheckPassword(User user, string password);

        Task<IList<Claim>> GetClaims(User user);
    }
}
