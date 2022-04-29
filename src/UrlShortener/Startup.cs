using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
        services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
        services.Configure<AuthSettings>(Configuration.GetSection("AuthSettings"));

        services.AddDbContextPool<AppDbContext>(options =>
            options.UseSqlServer(Configuration["AppSettings:SqlConnection"]));

        services.AddDependencyInjectionServices(Configuration);

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = "Bearer";
            options.DefaultChallengeScheme = "Bearer";
            options.DefaultAuthenticateScheme = "Bearer";

        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero, // FIX: might cause issues, if auth is out of sync
                ValidIssuer = Configuration["AuthSettings:JwtIssuer"],
                ValidAudience = Configuration["AuthSettings:JwtIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthSettings:JwtKey"]))
            };
        });

        services.AddControllers().AddFluentValidation();

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });

        services.AddSwagger();

        services.ConfigureOptions<ConfigureSwaggerOptions>();
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

        context.Database.Migrate();
        seeder.Seed(); // Order is important!

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