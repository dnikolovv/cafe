using Cafe.Api.Hateoas.Resources;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cafe.Tests.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<TResponse> ShouldDeserializeTo<TResponse>(this HttpResponseMessage response)
        {
            try
            {
                return await response.Content.ReadAsAsync<TResponse>();
            }
            catch (Exception e)
            {
                throw new ShouldAssertException($"Expected the response to be of type {typeof(TResponse).FullName} but could not deserialize it.", e);
            }
        }

        public static async Task<TResource> ShouldBeAResource<TResource>(this HttpResponseMessage response, IEnumerable<string> expectedLinks = null)
            where TResource : Resource
        {
            // We always expect resources to be returned with a success status code
            response.IsSuccessStatusCode.ShouldBeTrue();

            var resource = await response.ShouldDeserializeTo<TResource>();

            expectedLinks?.ShouldAllBe(el => resource.Links.Any(l => l.Key == el));

            return resource;
        }
    }
}
