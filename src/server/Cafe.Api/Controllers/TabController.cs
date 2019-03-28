using Cafe.Core.AuthContext;
using Cafe.Core.TabContext.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cafe.Api.Controllers
{
    [Authorize(Policy = AuthConstants.Policies.IsWaiter)]
    public class TabController : ApiController
    {
        private readonly IMediator _mediator;

        public TabController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Opens a new tab on a given table.
        /// </summary>
        [HttpPost("open")]
        public async Task<IActionResult> OpenTab([FromBody] OpenTab command) =>
            (await _mediator.Send(command))
            .Match(Ok, Error);

        /// <summary>
        /// Closes a tab.
        /// </summary>
        [HttpPost("close")]
        public async Task<IActionResult> CloseTab([FromBody] CloseTab command) =>
            (await _mediator.Send(command))
            .Match(Ok, Error);

        /// <summary>
        /// Orders a list of menu items for a given tab.
        /// </summary>
        [HttpPost("order")]
        public async Task<IActionResult> OrderMenuItems([FromBody] OrderMenuItems command) =>
            (await _mediator.Send(command))
            .Match(Ok, Error);

        /// <summary>
        /// Rejects a list of menu items for a given tab.
        /// </summary>
        [HttpPost("reject")]
        public async Task<IActionResult> RejectMenuItems([FromBody] RejectMenuItems command) =>
            (await _mediator.Send(command))
            .Match(Ok, Error);
    }
}
