using Cafe.Core.MenuContext.Commands;
using Cafe.Core.MenuContext.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cafe.Api.Controllers
{
    public class MenuController : ApiController
    {
        private readonly IMediator _mediator;

        public MenuController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves a list of all items currently in the menu.
        /// </summary>
        [HttpGet("items")]
        public async Task<IActionResult> GetMenuItems() =>
            (await _mediator.Send(new GetAllMenuItems()))
            .Match(Ok, Error);

        /// <summary>
        /// Adds items to the menu.
        /// </summary>
        /// <param name="command">The items to add.</param>
        [HttpPost("items")]
        public async Task<IActionResult> AddMenuItems([FromBody] AddMenuItems command) =>
            (await _mediator.Send(command))
            .Match(Ok, Error);
    }
}
