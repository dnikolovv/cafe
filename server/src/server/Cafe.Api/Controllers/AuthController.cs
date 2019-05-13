using Cafe.Api.Hateoas.Resources;
using Cafe.Api.Hateoas.Resources.Auth;
using Cafe.Core.AuthContext;
using Cafe.Core.AuthContext.Commands;
using Cafe.Core.AuthContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Views;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Optional.Async;
using System.Net;
using System.Threading.Tasks;

namespace Cafe.Api.Controllers
{
    public class AuthController : ApiController
    {
        public AuthController(IResourceMapper resourceMapper, IMediator mediator)
            : base(resourceMapper, mediator)
        {
        }

        /// <summary>
        /// Retrieves the currently logged in user.
        /// </summary>
        [HttpGet(Name = nameof(GetCurrentUser))]
        public async Task<IActionResult> GetCurrentUser() =>
            (await Mediator.Send(new GetUser { Id = CurrentUserId })
                .MapAsync(ToResourceAsync<UserView, UserResource>))
                .Match<IActionResult>(Ok, _ => Unauthorized());

        /// <summary>
        /// Login.
        /// </summary>
        /// <param name="command">The credentials.</param>
        /// <returns>A JWT.</returns>
        /// <response code="200">If the credentials have a match.</response>
        /// <response code="400">If the credentials don't match/don't meet the requirements.</response>
        [HttpPost("login", Name = nameof(Login))]
        [ProducesResponseType(typeof(JwtView), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody] Login command) =>
            (await Mediator.Send(command)
                .MapAsync(ToResourceAsync<JwtView, LoginResource>))
                .Match(
                jwt =>
                {
                    SetAuthCookie(jwt.TokenString);
                    return Ok(jwt);
                },
                Error);

        /// <summary>
        /// Logout. (unsets the auth cookie)
        /// </summary>
        [HttpDelete("logout", Name = nameof(Logout))]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete(AuthConstants.Cookies.AuthCookieName);
            return NoContent();
        }

        /// <summary>
        /// Register.
        /// </summary>
        /// <param name="command">The user model.</param>
        /// <returns>A user model.</returns>
        /// <response code="201">A user was created.</response>
        /// <response code="400">Invalid input.</response>
        [HttpPost("register", Name = nameof(Register))]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register([FromBody] Register command) =>
            (await Mediator.Send(command))
                .Match(u => CreatedAtAction(nameof(Register), u), Error);

        /// <summary>
        /// Retrieves all user accounts.
        /// </summary>
        [HttpGet("users", Name = nameof(GetAllUserAccounts))]
        [Authorize(Policy = AuthConstants.Policies.IsAdmin)]
        public async Task<IActionResult> GetAllUserAccounts() =>
            (await Mediator.Send(new GetAllUserAccounts()))
                .Match(Ok, Error);

        /// <summary>
        /// Assigns a waiter to an account.
        /// </summary>
        [HttpPost("assign/waiter")]
        [Authorize(Policy = AuthConstants.Policies.IsAdmin)]
        public async Task<IActionResult> AssignWaiterToAccount([FromBody] AssignWaiterToAccount command) =>
            (await Mediator.Send(command))
                .Match(Ok, Error);

        /// <summary>
        /// Assigns a manager to an account.
        /// </summary>
        [HttpPost("assign/manager")]
        [Authorize(Policy = AuthConstants.Policies.IsAdmin)]
        public async Task<IActionResult> AssignManagerToAccount([FromBody] AssignManagerToAccount command) =>
            (await Mediator.Send(command))
                .Match(Ok, Error);

        /// <summary>
        /// Assigns a cashier to an account.
        /// </summary>
        [HttpPost("assign/cashier")]
        [Authorize(Policy = AuthConstants.Policies.IsAdmin)]
        public async Task<IActionResult> AssignCashierToAccount([FromBody] AssignCashierToAccount command) =>
            (await Mediator.Send(command))
                .Match(Ok, Error);

        /// <summary>
        /// Assigns a barista to an account.
        /// </summary>
        [HttpPost("assign/barista")]
        [Authorize(Policy = AuthConstants.Policies.IsAdmin)]
        public async Task<IActionResult> AssignBaristaToAccount([FromBody] AssignBaristaToAccount command) =>
            (await Mediator.Send(command))
                .Match(Ok, Error);

        private void SetAuthCookie(string token) =>
            HttpContext.Response.Cookies.Append(AuthConstants.Cookies.AuthCookieName, token);
    }
}