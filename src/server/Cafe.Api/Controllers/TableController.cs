using Cafe.Core.TableContext.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cafe.Api.Controllers
{
    public class TableController : ApiController
    {
        private readonly IMediator _mediator;

        public TableController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Adds a table to the café.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddTable([FromBody] AddTable command) =>
            (await _mediator.Send(command))
            .Match(Ok, Error);
    }
}
