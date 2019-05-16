using Cafe.Api.Hateoas.Resources;
using Cafe.Api.Hateoas.Resources.Home;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cafe.Api.Controllers
{
    [Route("")]
    public class HomeController : ApiController
    {
        public HomeController(
            IResourceMapper resourceMapper,
            IMediator mediator)
            : base(resourceMapper, mediator)
        {
        }

        /// <summary>
        /// The root of the API.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ApiHome()
        {
            var result = await ToEmptyResourceAsync<ApiRootResource>(x => x.IsUserLoggedIn = User.Identity.IsAuthenticated);

            return Ok(result);
        }
    }
}
