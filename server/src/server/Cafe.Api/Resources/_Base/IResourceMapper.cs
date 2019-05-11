using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cafe.Api.Resources
{
    public interface IResourceMapper
    {
        Task<TResource> MapAsync<T, TResource>(T source)
            where TResource : Resource;

        Task<TContainer> MapContainerAsync<T, TResource, TContainer>(IEnumerable<T> models)
            where TContainer : ContainerResource<TResource>, new()
            where TResource : Resource;
    }
}
