using Microsoft.AspNetCore.Mvc;

namespace Cafe.Api.Controllers
{
    public class HealthController : ApiController
    {
        [HttpGet]
        public IActionResult Get() =>
            Ok();
    }
}
