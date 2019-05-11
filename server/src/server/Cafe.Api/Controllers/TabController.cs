using Cafe.Api.Resources;
using Cafe.Core.AuthContext;
using Cafe.Core.TabContext.Commands;
using Cafe.Core.TabContext.Queries;
using Cafe.Domain.Views;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Optional.Async;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cafe.Api.Controllers
{
    [Authorize(Policy = AuthConstants.Policies.IsAdminOrWaiter)]
    public class TabController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IResourceMapper _resourceMapper;

        public TabController(IMediator mediator, IResourceMapper resourceMapper)
        {
            _mediator = mediator;
            _resourceMapper = resourceMapper;
        }

        /// <summary>
        /// Retrieves a tab by id.
        /// </summary>
        [HttpGet("{id}", Name = nameof(GetTabView))]
        public async Task<IActionResult> GetTabView(Guid id) =>
            (await _mediator.Send(new GetTabView { Id = id })
                .MapAsync(ToResourceAsync<TabView, TabResource>))
                .Match(Ok, Error);

        protected Task<TResource> ToResourceAsync<T, TResource>(T obj)
            where TResource : Resource =>
            _resourceMapper.MapAsync<T, TResource>(obj);

        protected Task<TContainer> ToContainerResourceAsync<T, TNested, TContainer>(IEnumerable<T> models)
            where TContainer : Resource
            where TNested : Resource =>
            _resourceMapper.MapContainerAsync<T, TNested, TContainer>(models);

        /// <summary>
        /// Retrieves all open tabs.
        /// </summary>
        [HttpGet(Name = nameof(GetAllOpenTabs))]
        public async Task<IActionResult> GetAllOpenTabs() =>
            (await _mediator.Send(new GetAllOpenTabs())
                .MapAsync(ToContainerResourceAsync<TabView, TabResource, TabsResource>))
                .Match(Ok, Error);

        /// <summary>
        /// Opens a new tab on a given table.
        /// </summary>
        [HttpPost("open", Name = nameof(OpenTab))]
        public async Task<IActionResult> OpenTab([FromBody] OpenTab command) =>
            (await _mediator.Send(command))
            .Match(Ok, Error);

        /// <summary>
        /// Closes a tab.
        /// </summary>
        [HttpPut("close", Name = nameof(CloseTab))]
        public async Task<IActionResult> CloseTab([FromBody] CloseTab command) =>
            (await _mediator.Send(command))
            .Match(Ok, Error);

        /// <summary>
        /// Orders a list of menu items for a given tab.
        /// </summary>
        [HttpPut("order", Name = nameof(OrderMenuItems))]
        public async Task<IActionResult> OrderMenuItems([FromBody] OrderMenuItems command) =>
            (await _mediator.Send(command))
            .Match(Ok, Error);

        /// <summary>
        /// Serves a list of menu items.
        /// </summary>
        [HttpPut("serve", Name = nameof(ServeMenuItems))]
        public async Task<IActionResult> ServeMenuItems([FromBody] ServeMenuItems command) =>
            (await _mediator.Send(command))
            .Match(Ok, Error);

        /// <summary>
        /// Rejects a list of menu items for a given tab.
        /// </summary>
        [HttpPut("reject", Name = nameof(RejectMenuItems))]
        public async Task<IActionResult> RejectMenuItems([FromBody] RejectMenuItems command) =>
            (await _mediator.Send(command))
            .Match(Ok, Error);
    }
}
