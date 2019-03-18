using Cafe.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Cafe.Api.Controllers
{
    [Route("api/[controller]")]
    public class ApiController : Controller
    {
        protected IActionResult Error(Error error) =>
            new BadRequestObjectResult(error);
    }
}
