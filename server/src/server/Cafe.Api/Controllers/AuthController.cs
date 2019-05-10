using Cafe.Core.AuthContext;
using Cafe.Core.AuthContext.Commands;
using Cafe.Core.AuthContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Views;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Cafe.Api.Controllers
{
    public class AuthController : ApiController
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves the currently logged in user.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser() =>
            (await _mediator.Send(new GetUser { Id = CurrentUserId }))
                .Match<IActionResult>(Ok, _ => Unauthorized());

        /// <summary>
        /// Login.
        /// </summary>
        /// <param name="command">The credentials.</param>
        /// <returns>A JWT.</returns>
        /// <response code="200">If the credentials have a match.</response>
        /// <response code="400">If the credentials don't match/don't meet the requirements.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(JwtView), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody] Login command) =>
            (await _mediator.Send(command))
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
        [HttpDelete("logout")]
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
        [HttpPost("register")]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register([FromBody] Register command) =>
            (await _mediator.Send(command))
                .Match(u => CreatedAtAction(nameof(Register), u), Error);

        /// <summary>
        /// Retrieves all user accounts.
        /// </summary>
        [HttpGet("users")]
        [Authorize(Policy = AuthConstants.Policies.IsAdmin)]
        public async Task<IActionResult> GetAllUserAccounts() =>
            (await _mediator.Send(new GetAllUserAccounts()))
                .Match(Ok, Error);

        /// <summary>
        /// Assigns a waiter to an account.
        /// </summary>
        [HttpPost("assign/waiter")]
        [Authorize(Policy = AuthConstants.Policies.IsAdmin)]
        public async Task<IActionResult> AssignWaiterToAccount([FromBody] AssignWaiterToAccount command) =>
            (await _mediator.Send(command))
                .Match(Ok, Error);

        /// <summary>
        /// Assigns a manager to an account.
        /// </summary>
        [HttpPost("assign/manager")]
        [Authorize(Policy = AuthConstants.Policies.IsAdmin)]
        public async Task<IActionResult> AssignManagerToAccount([FromBody] AssignManagerToAccount command) =>
            (await _mediator.Send(command))
                .Match(Ok, Error);

        /// <summary>
        /// Assigns a cashier to an account.
        /// </summary>
        [HttpPost("assign/cashier")]
        [Authorize(Policy = AuthConstants.Policies.IsAdmin)]
        public async Task<IActionResult> AssignCashierToAccount([FromBody] AssignCashierToAccount command) =>
            (await _mediator.Send(command))
                .Match(Ok, Error);

        /// <summary>
        /// Assigns a barista to an account.
        /// </summary>
        [HttpPost("assign/barista")]
        [Authorize(Policy = AuthConstants.Policies.IsAdmin)]
        public async Task<IActionResult> AssignBaristaToAccount([FromBody] AssignBaristaToAccount command) =>
            (await _mediator.Send(command))
                .Match(Ok, Error);

        private void SetAuthCookie(string token) =>
            HttpContext.Response.Cookies.Append(AuthConstants.Cookies.AuthCookieName, token);
    }
}