using Cafe.Core.MenuContext.Commands;
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
        /// Adds items to the menu.
        /// </summary>
        /// <param name="command">The items to add.</param>
        [HttpPost("items")]
        public async Task<IActionResult> AddMenuItems([FromBody] AddMenuItems command) =>
            (await _mediator.Send(command))
            .Match(Ok, Error);
    }
}
