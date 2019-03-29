using Cafe.Core.AuthContext;
using Microsoft.AspNetCore.Authorization;
using System;

namespace Cafe.Api.Configuration
{
    public static class Extensions
    {
        public static void IsAdminOr(this AuthorizationPolicyBuilder policyBuilder, Func<AuthorizationHandlerContext, bool> condition) =>
            policyBuilder.RequireAssertion(ctx =>
                ctx.User.HasClaim(AuthConstants.ClaimTypes.IsAdmin, true.ToString()) ||
                condition(ctx));
    }
}
