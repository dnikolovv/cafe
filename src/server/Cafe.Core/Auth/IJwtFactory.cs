using System.Collections.Generic;
using System.Security.Claims;

namespace Cafe.Core.Auth
{
    public interface IJwtFactory
    {
        string GenerateEncodedToken(string userId, string email, IEnumerable<Claim> additionalClaims);
    }
}
