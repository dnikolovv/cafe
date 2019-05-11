using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cafe.Api.Resources
{
    public interface IResourceMapper
    {
        Task<TResource> MapAsync<T, TResource>(T source)
            where TResource : Resource;

        Task<TContainerResource> MapContainerAsync<TModel, TNestedResource, TContainerResource>(IEnumerable<TModel> models)
            where TContainerResource : Resource
            where TNestedResource : Resource;
    }
}
