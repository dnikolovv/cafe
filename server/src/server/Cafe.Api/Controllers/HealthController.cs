using Microsoft.AspNetCore.Mvc;

namespace Cafe.Api.Controllers
{
    public class HealthController : Controller
    {
        [HttpGet]
        public IActionResult Get() =>
            Ok();
    }
}
