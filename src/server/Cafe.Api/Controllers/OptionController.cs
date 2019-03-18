using Microsoft.AspNetCore.Mvc;
using Optional;

namespace Cafe.Api.Controllers
{
    public class OptionController : ApiController
    {
        /// <summary>
        /// Demonstrates the Option model binding with query string parameters.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <returns>A model showing the bound data.</returns>
        /// <response code="200">The model was bound successfully.</response>
        /// <response code="400">When it could not parse some of the inputted data.</response>
        [HttpGet]
        public IActionResult Get([FromQuery] ParametersDemo request) =>
            Ok(new
            {
                message = "You gave me query parameters:",
                text = request.Text.ToString(),
                number = request.Number.ToString(),
                flag = request.Flag.ToString()
            });

        /// <summary>
        /// This class is for demonstration purposes.
        /// The optional values need to be put in as class properties in order
        /// for Swagger to be able to display them properly. There is nothing wrong with
        /// having something like Get([FromQuery] Option&lt;string&gt; text), it's going to work,
        /// it's just that it will not be properly displayed in the Swagger UI.
        /// </summary>
        public class ParametersDemo
        {
            public Option<string> Text { get; set; }

            public Option<int> Number { get; set; }

            public Option<bool> Flag { get; set; }
        }
    }
}
