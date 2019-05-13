using AutoFixture;
using Cafe.Core.AuthContext.Commands;
using Cafe.Tests.Business.AuthContext;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cafe.Tests.Api
{
    public class ApiTestsHelper
    {
        private readonly AppFixture _appFixture;
        private readonly AuthTestsHelper _authHelper;

        public ApiTestsHelper(AppFixture appFixture)
        {
            _appFixture = appFixture;
            _authHelper = new AuthTestsHelper(_appFixture);
        }

        public async Task InTheContextOfAnAuthenticatedUser(Func<HttpClient, Task> serverCall, Fixture fixture, IEnumerable<Claim> withClaims = null)
        {
            var registerCommand = fixture
                .Create<Register>();

            await _authHelper.Register(registerCommand);

            if (withClaims != null)
                await _authHelper.AddClaimsAsync(registerCommand.Id, withClaims);

            var token = (await _authHelper
                .Login(registerCommand.Email, registerCommand.Password))
                .TokenString;

            await _appFixture.ExecuteHttpClientAsync(serverCall, token);
        }
    }
}
