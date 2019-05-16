using Cafe.Api.Hateoas.Resources;
using Cafe.Core.AuthContext;
using Cafe.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Optional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cafe.Api.Controllers
{
    [Route("[controller]")]
    public class ApiController : Controller
    {
        public ApiController(IResourceMapper resourceMapper, IMediator mediator)
        {
            ResourceMapper = resourceMapper;
            Mediator = mediator;
        }

        protected Option<Guid> BaristaId => TryGetGuidClaim(AuthConstants.ClaimTypes.BaristaId);
        protected Guid CurrentUserId => TryGetGuidClaim(ClaimTypes.NameIdentifier).ValueOr(Guid.Empty);

        protected IMediator Mediator { get; }
        protected IResourceMapper ResourceMapper { get; }

        protected IActionResult Error(Error error)
        {
            switch (error.Type)
            {
                case ErrorType.Validation:
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

        protected IActionResult NotFound(Error error) =>
            NotFound((object)error);

        protected Task<TContainer> ToResourceContainerAsync<T, TResource, TContainer>(IEnumerable<T> models)
            where TContainer : ResourceContainer<TResource>, new()
            where TResource : Resource =>
            ResourceMapper.MapContainerAsync<T, TResource, TContainer>(models);

        protected Task<TResource> ToResourceAsync<T, TResource>(T obj)
            where TResource : Resource =>
            ResourceMapper.MapAsync<T, TResource>(obj);

        protected Task<TResource> ToEmptyResourceAsync<TResource>(Unit unit = default(Unit))
            where TResource : Resource, new() =>
            ResourceMapper.CreateEmptyResourceAsync<TResource>();

        protected Task<TResource> ToEmptyResourceAsync<TResource>(Action<TResource> beforeMap)
            where TResource : Resource, new() =>
            ResourceMapper.CreateEmptyResourceAsync(beforeMap);

        private Option<Guid> TryGetGuidClaim(string claimType)
        {
            var claimValue = User
                .Claims
                .FirstOrDefault(c => c.Type == claimType)?
                .Value;

            return claimValue
                .SomeNotNull()
                .Filter(v => Guid.TryParse(v, out Guid _))
                .Map(v => new Guid(v));
        }
    }
}