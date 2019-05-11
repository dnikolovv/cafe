using AutoMapper;
using RiskFirst.Hateoas;
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
            var resource = _mapper.Map<TResource>(source);
            await _linksService.AddLinksAsync(resource);
            return resource;
        }

        public async Task<TContainerResource> MapContainerAsync<TModel, TNestedResource, TContainerResource>(IEnumerable<TModel> models)
            where TContainerResource : Resource
            where TNestedResource : Resource
        {
            var nestedResources = await Task.WhenAll(models
                .Select(async m =>
                {
                    var nestedResource = _mapper.Map<TNestedResource>(m);
                    await _linksService.AddLinksAsync(nestedResource);
                    return nestedResource;
                }));

            var container = _mapper.Map<TContainerResource>(nestedResources);

            await _linksService.AddLinksAsync(container);

            return container;
        }
    }
}
