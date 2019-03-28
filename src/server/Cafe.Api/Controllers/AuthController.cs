using Cafe.Core.AuthContext;
using Cafe.Core.AuthContext.Commands;
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
            .Match(Ok, Error);

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
    }
}