using Microsoft.IdentityModel.Tokens;
using System;

namespace Cafe.Core.Auth.Configuration
{
    public class JwtConfiguration
    {
        /// <summary>
        /// Gets or sets the "iss" (Issuer) Claim - The "iss" (issuer) claim identifies the principal that issued the JWT.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the "sub" (Subject) Claim - The "sub" (subject) claim identifies the principal that is the subject of the JWT.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the "aud" (Audience) Claim - The "aud" (audience) claim identifies the recipients that the JWT is intended for.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Gets or sets the timespan the token will be valid for (default is 120 min)
        /// </summary>
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(120);

        /// <summary>
        /// Gets the "jti" (JWT ID) Claim (default ID is a GUID)
        /// </summary>
        public Func<string> JtiGenerator =>
          () => Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the signing key to use when generating tokens.
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }
    }
}
