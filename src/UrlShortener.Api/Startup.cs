using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Models.App;
using UrlShortener.Api.Extensions.Startup;

namespace UrlShortener.Api;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureSettings(Configuration);
        services.AddValidators();
        services.RegisterServices(Configuration);
        services.AddCache(Configuration);
        services.ConfigureDbContext(Configuration);
        services.AddAuthentication(Configuration);
        services.AddControllers().AddFluentValidation();
        services.AddVersioning();
        services.AddSwagger();
#if RELEASE
        services.AddMetrics();
        services.AddAppMetricsHealthPublishing();
#endif
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider,
        AppDbContext context)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
            });
        }

        if (context.Database.IsRelational())
            context.Database.Migrate();

        app.UseHealthChecksEndpoint();

        app.UseHsts();
        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}