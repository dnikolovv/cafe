using Cafe.Api.Filters;
using Cafe.Domain;
using Cafe.Tests.Customizations;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Cafe.Tests.Api.Filters
{
    public class ModelStateFilterTests
    {
        [Fact]
        public void ShouldDoNothingWhenThereAreNoModelStateErrors()
        {
            // Arrange
            var context = FilterContextProvider.GetActionExecutingContext("POST");

            // Purposefully not adding any errors
            var filter = new ModelStateFilter();

            // Act
            filter.OnActionExecuting(context);

            // Assert
            context.Result.ShouldNotBeOfType<BadRequestObjectResult>();
        }

        [Theory]
        [CustomizedAutoData]
        public void ShouldReturnBadRequestWhenThereAreModelStateErrros(Dictionary<string, string> errors)
        {
            // Arrange
            var context = FilterContextProvider.GetActionExecutingContext("POST");

            foreach (var e in errors)
            {
                context.ModelState.AddModelError(e.Key, e.Value);
            }

            var filter = new ModelStateFilter();

            // Act
            filter.OnActionExecuting(context);

            // Assert
            var result = context.Result.ShouldBeOfType<BadRequestObjectResult>();
            var error = result.Value.ShouldBeOfType<Error>();

            var expectedErrorMessages = errors
                .Select(e => e.Value)
                .ToArray();

            error.Messages.ShouldAllBe(e => expectedErrorMessages.Contains(e));
        }
    }
}
