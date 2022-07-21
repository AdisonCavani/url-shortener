using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace UrlShortener.Api.Extensions.Startup;

public static class SwaggerService
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.ExampleFilters();
            options.EnableAnnotations();
            options.DescribeAllParametersInCamelCase();
            options.OperationFilter<AuthOperationFilter>();

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);

            options.IncludeXmlComments(xmlPath);

            // Configure Auth
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new()
            {
                Description = "JWT Authorization header using the bearer scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
        });

        services.AddSwaggerExamplesFromAssemblyOf<Api.Startup>();
        services.ConfigureOptions<ConfigureSwaggerOptions>();
    }
}
