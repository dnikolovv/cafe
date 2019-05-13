using Cafe.Api.Hateoas.Resources;
using Cafe.Core.AuthContext;
using Cafe.Core.BaristaContext.Commands;
using Cafe.Core.BaristaContext.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cafe.Api.Controllers
{
    [Authorize(Policy = AuthConstants.Policies.IsAdminOrBarista)]
    public class BaristaController : ApiController
    {
        public BaristaController(IResourceMapper resourceMapper, IMediator mediator)
            : base(resourceMapper, mediator)
        {
        }

        /// <summary>
        /// Hires a new barista.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> HireBarista([FromBody] HireBarista command) =>
            (await Mediator.Send(command))
                .Match(Ok, Error);

        /// <summary>
        /// Retrieves currently employed baristas.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetEmployedBaristas() =>
            (await Mediator.Send(new GetEmployedBaristas()))
                .Match(Ok, Error);
    }
}
