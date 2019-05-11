using AutoMapper;
using RiskFirst.Hateoas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cafe.Api.Resources
{
    public class ResourceMapper : IResourceMapper
    {
        private readonly IMapper _mapper;
        private readonly ILinksService _linksService;

        public ResourceMapper(IMapper mapper, ILinksService linksService)
        {
            _mapper = mapper;
            _linksService = linksService;
        }

        public async Task<TResource> MapAsync<T, TResource>(T source)
            where TResource : Resource
        {
            try
            {
                var resource = _mapper.Map<TResource>(source);
                await _linksService.AddLinksAsync(resource);
                return resource;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    "A problem occured while trying to convert a type to a resource. " +
                    "Make sure that you have set up AutoMapper properly and your link policies don't " +
                    "contain any unexisting routes.",
                    e);
            }
        }

        public async Task<TContainer> MapContainerAsync<T, TResource, TContainer>(IEnumerable<T> models)
            where TContainer : ContainerResource<TResource>, new()
            where TResource : Resource
        {
            var nestedResources = await Task.WhenAll(models
                .Select(MapAsync<T, TResource>));

            var container = new TContainer
            {
                Items = nestedResources
            };

            await _linksService.AddLinksAsync(container);

            return container;
        }
    }
}
