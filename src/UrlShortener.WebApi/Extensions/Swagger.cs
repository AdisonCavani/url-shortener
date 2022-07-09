using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace UrlShortener.WebApi.Extensions;

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

        services.AddSwaggerExamplesFromAssemblyOf<Startup>();
        services.ConfigureOptions<ConfigureSwaggerOptions>();
    }
}
