using Cafe.Api.Hateoas.Resources;
using Cafe.Api.Hateoas.Resources.Menu;
using Cafe.Core.AuthContext;
using Cafe.Core.MenuContext.Commands;
using Cafe.Core.MenuContext.Queries;
using Cafe.Domain.Views;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Optional.Async;
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
        [HttpGet("items", Name = nameof(GetMenuItems))]
        public Task<IActionResult> GetMenuItems() =>
            ResourceContainerResult<MenuItemView, MenuItemResource, MenuItemsContainerResource>(new GetAllMenuItems());

        /// <summary>
        /// Adds items to the menu.
        /// </summary>
        /// <param name="command">The items to add.</param>
        [HttpPost("items", Name = nameof(AddMenuItems))]
        [Authorize(Policy = AuthConstants.Policies.IsAdminOrManager)]
        public async Task<IActionResult> AddMenuItems([FromBody] AddMenuItems command) =>
            (await Mediator.Send(command)
                .MapAsync(ToEmptyResourceAsync<AddMenuItemsResource>))
                .Match(Ok, Error);
    }
}
