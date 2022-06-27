using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Extensions;
using UrlShortener.Models.App;
using UrlShortener.Services;

namespace UrlShortener;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    // Use this method to add services to the container.
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
    }

    // Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider, AppDbContext context, AccountSeeder seeder)
    {
        // Configure the HTTP request pipeline.
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
        {
            context.Database.Migrate();
            seeder.Seed(); // Order is important!
        }

        // Client side
        app.UseHsts();

        // Server side
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

}