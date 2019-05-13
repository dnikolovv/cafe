using AutoFixture;
using Cafe.Core.AuthContext;
using Cafe.Core.AuthContext.Commands;
using Cafe.Core.ManagerContext.Commands;
using Cafe.Core.TableContext.Commands;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Domain.Entities;
using Cafe.Tests.Business.AuthContext;
using Cafe.Tests.Business.TabContext.Helpers;
using Microsoft.EntityFrameworkCore;
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
        private readonly TabTestsHelper _tabHelper;

        public ApiTestsHelper(AppFixture appFixture)
        {
            _appFixture = appFixture;
            _authHelper = new AuthTestsHelper(_appFixture);
            _tabHelper = new TabTestsHelper(_appFixture);
        }

        public async Task InTheContextOfAnAuthenticatedUser(Func<HttpClient, Task> serverCall, Fixture fixture, IEnumerable<Claim> withClaims = null)
        {
            var token = await SetupUserWithClaims(fixture, withClaims);

            await _appFixture.ExecuteHttpClientAsync(serverCall, token);
        }

        public async Task InTheContextOfAWaiter(Func<Waiter, Func<HttpClient, Task>> serverCall, Fixture fixture)
        {
            var hireWaiter = fixture.Create<HireWaiter>();

            await _tabHelper.SetupWaiterWithTable(hireWaiter, fixture.Create<AddTable>());

            var waiter = await _appFixture.ExecuteDbContextAsync(dbContext =>
                dbContext
                    .Waiters
                    .Include(w => w.ServedTables)
                    .FirstAsync(w => w.Id == hireWaiter.Id));

            var token = await SetupUserWithClaims(
                fixture,
                new List<Claim> { new Claim(AuthConstants.ClaimTypes.WaiterId, waiter.Id.ToString()) });

            await _appFixture.ExecuteHttpClientAsync(serverCall(waiter), token);
        }

        public async Task InTheContextOfAManager(Func<Manager, Func<HttpClient, Task>> serverCall, Fixture fixture)
        {
            var hireManager = fixture.Create<HireManager>();

            await _appFixture.SendAsync(hireManager);

            var manager = await _appFixture.ExecuteDbContextAsync(dbContext =>
                dbContext
                    .Managers
                    .FirstAsync(m => m.Id == hireManager.Id));

            var token = await SetupUserWithClaims(
                fixture,
                new List<Claim> { new Claim(AuthConstants.ClaimTypes.ManagerId, manager.Id.ToString()) });

            await _appFixture.ExecuteHttpClientAsync(serverCall(manager), token);
        }

        private async Task<string> SetupUserWithClaims(Fixture fixture, IEnumerable<Claim> withClaims)
        {
            var registerCommand = fixture
                .Create<Register>();

            await _authHelper.Register(registerCommand);

            if (withClaims != null)
                await _authHelper.AddClaimsAsync(registerCommand.Id, withClaims);

            var token = (await _authHelper
                .Login(registerCommand.Email, registerCommand.Password))
                .TokenString;

            return token;
        }
    }
}
