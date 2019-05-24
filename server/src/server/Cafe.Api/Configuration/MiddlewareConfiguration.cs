using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Cafe.Api.Configuration
{
    public static class MiddlewareConfiguration
    {
        public static void UseSwagger(this IApplicationBuilder app, string endpointName)
        {
            app.UseSwagger();
            app.UseSwaggerOn("swagger", endpointName);
            app.UseSwaggerOn("openapi", endpointName);
        }

        public static void AddLogging(this ILoggerFactory loggerFactory, IConfigurationSection loggingConfiguration)
        {
            loggerFactory.AddConsole(loggingConfiguration);
            loggerFactory.AddFile("logs/cafe-api-{Date}.log");
            loggerFactory.AddDebug();
        }

        private static void UseSwaggerOn(this IApplicationBuilder app, string route, string endpointName)
        {
            app.UseSwaggerUI(setup =>
            {
                setup.RoutePrefix = route;

                setup.IndexStream = () => typeof(Startup)
                    .Assembly
                    .GetManifestResourceStream("Cafe.Api.Resources.Swagger.index.html");

                setup.SwaggerEndpoint(
                    url: "/swagger/v1/swagger.json",
                    name: endpointName);
            });
        }
    }
}
