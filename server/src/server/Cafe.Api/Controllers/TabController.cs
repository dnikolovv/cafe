using Cafe.Core.AuthContext;
using Cafe.Core.TabContext.Commands;
using Cafe.Core.TabContext.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cafe.Api.Controllers
{
    [Authorize(Policy = AuthConstants.Policies.IsAdminOrWaiter)]
    public class TabController : ApiController
    {
        private readonly IMediator _mediator;

        public TabController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves a tab by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTabView(Guid id) =>
            (await _mediator.Send(new GetTabView { Id = id }))
            .Match(Ok, Error);

        /// <summary>
        /// Retrieves all open tabs.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllOpenTabs() =>
            (await _mediator.Send(new GetAllOpenTabs()))
            .Match(Ok, Error);

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
        [HttpPut("close")]
        public async Task<IActionResult> CloseTab([FromBody] CloseTab command) =>
            (await _mediator.Send(command))
            .Match(Ok, Error);

        /// <summary>
        /// Orders a list of menu items for a given tab.
        /// </summary>
        [HttpPut("order")]
        public async Task<IActionResult> OrderMenuItems([FromBody] OrderMenuItems command) =>
            (await _mediator.Send(command))
            .Match(Ok, Error);

        /// <summary>
        /// Rejects a list of menu items for a given tab.
        /// </summary>
        [HttpPut("reject")]
        public async Task<IActionResult> RejectMenuItems([FromBody] RejectMenuItems command) =>
            (await _mediator.Send(command))
            .Match(Ok, Error);
    }
}
