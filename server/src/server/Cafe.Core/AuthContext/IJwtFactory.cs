using System.Collections.Generic;
using System.Security.Claims;

namespace Cafe.Core.AuthContext
{
    public interface IJwtFactory
    {
        string GenerateEncodedToken(string userId, string email, IEnumerable<Claim> additionalClaims);
    }
}
