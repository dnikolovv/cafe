using Cafe.Core.AuthContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System;

namespace Cafe.Api.Configuration
{
    public static class Extensions
    {
        public static void IsAdminOr(this AuthorizationPolicyBuilder policyBuilder, Func<AuthorizationHandlerContext, bool> condition) =>
            policyBuilder.RequireAssertion(ctx =>
                ctx.User.HasClaim(AuthConstants.ClaimTypes.IsAdmin, true.ToString()) ||
                condition(ctx));

        public static (string Email, string Password) GetAdminCredentials(this IConfiguration configuration)
        {
            var adminSection = configuration.GetSection("DefaultAdminAccount");

            var adminEmail = adminSection["Email"];
            var adminPassword = adminSection["Password"];

            return (adminEmail, adminPassword);
        }
    }
}
