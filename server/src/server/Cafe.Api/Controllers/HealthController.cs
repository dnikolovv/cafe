using Microsoft.AspNetCore.Mvc;

namespace Cafe.Api.Controllers
{
    public class HealthController : Controller
    {
        [HttpGet("health")]
        public IActionResult Get() =>
            Ok();
    }
}
