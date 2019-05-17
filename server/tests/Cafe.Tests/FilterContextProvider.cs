using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;

namespace Cafe.Tests
{
    public static class FilterContextProvider
    {
        public static ActionExecutingContext GetActionExecutingContext(string requestMethod)
        {
            var actionContext = GetFakeActionContext(requestMethod);

            var filterContext = A.Fake<ActionExecutingContext>(context => context.WithArgumentsForConstructor(() => new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                A.Fake<Controller>())));

            return filterContext;
        }

        public static ExceptionContext GetExceptionContext(Exception exception)
        {
            var actionContext = GetFakeActionContext();

            var exceptionContext = A.Fake<ExceptionContext>(context => context.WithArgumentsForConstructor(() => new ExceptionContext(
                actionContext,
                new List<IFilterMetadata>())));

            exceptionContext.Exception = exception;

            return exceptionContext;
        }

        private static ActionContext GetFakeActionContext(string requestMethod = null)
        {
            var httpContext = A.Fake<HttpContext>();
            httpContext.Request.Method = requestMethod ?? "GET";

            var actionContext = new ActionContext()
            {
                HttpContext = httpContext,
                RouteData = A.Fake<RouteData>(),
                ActionDescriptor = A.Fake<ActionDescriptor>()
            };
            return actionContext;
        }
    }
}