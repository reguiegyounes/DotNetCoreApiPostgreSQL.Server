using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCoreApiPostgreSQL.Api.Extensions
{
    public static class OpenApiFactory
    {
        public static IServiceCollection AddOpenApi(this IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(options =>
            {

            });
            return services;
        }

        public static IApplicationBuilder ConfigureOpenApi(this IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
            app.UseSwaggerUI();
            return app;
        }
    }
}
