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
            app.UseSwaggerUI(setup =>
            {
                setup.RoutePrefix = "swagger";

                setup.SwaggerEndpoint(
                    url: "/swagger/v1/swagger.json",
                    name: endpointName);
            });

            app.UseSwaggerUI(setup =>
            {
                setup.RoutePrefix = "openapi";

                setup.SwaggerEndpoint(
                    url: "/swagger/v1/swagger.json",
                    name: endpointName);
            });
        }

        public static void AddLogging(this ILoggerFactory loggerFactory, IConfigurationSection loggingConfiguration)
        {
            loggerFactory.AddConsole(loggingConfiguration);
            loggerFactory.AddFile("logs/cafe-api-{Date}.log");
            loggerFactory.AddDebug();
        }
    }
}
