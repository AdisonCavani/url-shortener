namespace ManagementService.Startup;

public static class Swagger
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.UseApiEndpoints();
            options.DescribeAllParametersInCamelCase();
        });
    }
}