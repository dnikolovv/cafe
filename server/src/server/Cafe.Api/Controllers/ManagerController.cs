using Cafe.Api.Hateoas.Resources;
using Cafe.Api.Hateoas.Resources.Manager;
using Cafe.Core.AuthContext;
using Cafe.Core.ManagerContext.Commands;
using Cafe.Core.ManagerContext.Queries;
using Cafe.Domain.Views;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Optional.Async;
using System.Threading.Tasks;

namespace Cafe.Api.Controllers
{
    [Authorize(Policy = AuthConstants.Policies.IsAdmin)]
    public class ManagerController : ApiController
    {
        public ManagerController(IResourceMapper resourceMapper, IMediator mediator)
            : base(resourceMapper, mediator)
        {
        }

        /// <summary>
        /// Retrieves all currently employed managers.
        /// </summary>
        [HttpGet(Name = nameof(GetEmployedManagers))]
        public Task<IActionResult> GetEmployedManagers() =>
            ResourceContainerResult<ManagerView, ManagerResource, ManagerContainerResource>(new GetEmployedManagers());

        /// <summary>
        /// Hires a new manager.
        /// </summary>
        [HttpPost(Name = nameof(HireManager))]
        public async Task<IActionResult> HireManager([FromBody] HireManager command) =>
            (await Mediator.Send(command)
                .MapAsync(ToEmptyResourceAsync<HireManagerResource>))
                .Match(Ok, Error);
    }
}
