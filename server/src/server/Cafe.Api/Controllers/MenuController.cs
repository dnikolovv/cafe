using Cafe.Api.Resources;
using Cafe.Core.AuthContext;
using Cafe.Core.MenuContext.Commands;
using Cafe.Core.MenuContext.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cafe.Api.Controllers
{
    public class MenuController : ApiController
    {
        public MenuController(IResourceMapper resourceMapper, IMediator mediator)
            : base(resourceMapper, mediator)
        {
        }

        /// <summary>
        /// Retrieves a list of all items currently in the menu.
        /// </summary>
        [HttpGet("items")]
        public async Task<IActionResult> GetMenuItems() =>
            (await Mediator.Send(new GetAllMenuItems()))
            .Match(Ok, Error);

        /// <summary>
        /// Adds items to the menu.
        /// </summary>
        /// <param name="command">The items to add.</param>
        [HttpPost("items")]
        [Authorize(Policy = AuthConstants.Policies.IsAdminOrManager)]
        public async Task<IActionResult> AddMenuItems([FromBody] AddMenuItems command) =>
            (await Mediator.Send(command))
            .Match(Ok, Error);
    }
}
