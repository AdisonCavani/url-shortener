using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Extensions.Startup;
using UrlShortener.Api.Models.App;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Program
builder.Host.AddSerilog();
builder.Host.AddAwsParameterStore();
#if RELEASE
builder.Host.AddAppMetrics()
#endif

// Configure services
builder.Services.ConfigureSettings(config);
builder.Services.AddValidators();
builder.Services.RegisterServices(config);
builder.Services.AddCache(config);
builder.Services.ConfigureDbContext(config);
builder.Services.AddAuthentication(config);
builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddVersioning();
builder.Services.AddSwagger();

#if RELEASE
builder.Services.AddMetrics();
builder.Services.AddAppMetricsHealthPublishing();
#endif

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        
        foreach (var description in provider.ApiVersionDescriptions)
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
    });
}

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

if (context.Database.IsRelational())
    context.Database.Migrate();

app.UseHealthChecksEndpoint();

app.UseHsts();
app.UseHttpsRedirection();

app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

public partial class Program { }