using Cafe.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Cafe.Api.Controllers
{
    [Route("api/[controller]")]
    public class ApiController : Controller
    {
        protected IActionResult Error(Error error)
        {
            switch (error.Type)
            {
                case ErrorType.ValidationError:
                    return BadRequest(error);
                case ErrorType.NotFound:
                    return NotFound(error);
                case ErrorType.Unauthorized:
                    return Unauthorized(error);
                case ErrorType.Conflict:
                    return Conflict(error);
                case ErrorType.Critical:
                    // This shouldn't really happen as critical errors are there to be used by the generic exception filter
                    return new ObjectResult(error)
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                default:
                    return BadRequest(error);
            }
        }
    }
}
