using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace UrlShortener.Versioning;

public static class SwaggerService
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddSwaggerGen(options =>
        {
            // TODO: configure swagger
            options.ExampleFilters();

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);

            options.IncludeXmlComments(xmlPath);
        });

        services.AddSwaggerExamplesFromAssemblyOf<Startup>();

        return services;
    }
}
