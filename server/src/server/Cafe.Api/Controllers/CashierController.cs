using Cafe.Api.Hateoas.Resources;
using Cafe.Api.Hateoas.Resources.Cashier;
using Cafe.Core.AuthContext;
using Cafe.Core.CashierContext.Commands;
using Cafe.Core.CashierContext.Queries;
using Cafe.Domain.Views;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Optional.Async;
using System.Threading.Tasks;

namespace Cafe.Api.Controllers
{
    public class CashierController : ApiController
    {
        public CashierController(IResourceMapper resourceMapper, IMediator mediator)
            : base(resourceMapper, mediator)
        {
        }

        /// <summary>
        /// Retrieves a list of all currently employed cashiers in the café.
        /// </summary>
        [HttpGet(Name = nameof(GetEmployedCashiers))]
        [Authorize(Policy = AuthConstants.Policies.IsAdminOrManager)]
        public async Task<IActionResult> GetEmployedCashiers() =>
            (await Mediator.Send(new GetEmployedCashiers())
                .MapAsync(ToResourceContainerAsync<CashierView, CashierResource, CashierContainerResource>))
                .Match(Ok, Error);

        /// <summary>
        /// Hires a cashier in the café.
        /// </summary>
        [HttpPost(Name = nameof(HireCashier))]
        [Authorize(Policy = AuthConstants.Policies.IsAdminOrManager)]
        public async Task<IActionResult> HireCashier([FromBody] HireCashier command) =>
            (await Mediator.Send(command)
                .MapAsync(ToEmptyResourceAsync<HireCashierResource>))
                .Match(Ok, Error);
    }
}