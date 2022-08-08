using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using UrlService.Database;
using UrlService.Startup;

var builder = WebApplication.CreateBuilder(args);

// Program
var settings = builder.GetAllOptions();
builder.WebHost.AddAwsParameterStore();
builder.WebHost.AddSerilog(builder.Environment, settings.LoggingOptions);

if (builder.Environment.IsProduction())
    builder.WebHost.AddAppMetrics();

// Configure services
builder.Services.ConfigureOptions(builder.Configuration, builder.Environment);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(settings.ConnectionOptions.PostgresConnectionString));

builder.Services.ConfigureDbContext(settings.ConnectionOptions);
builder.Services.AddCache(settings.ConnectionOptions);
builder.Services.AddValidators();
builder.Services.RegisterServices(settings.ConnectionOptions);
builder.Services.AddAuthentication(settings.AuthOptions);
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwagger();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => { policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
});

if (builder.Environment.IsProduction())
{
    builder.Services.AddMetrics();
    builder.Services.AddAppMetricsHealthPublishing();
}

var app = builder.Build();

// Configure
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.SeedDataAsync();
app.UseHealthChecksEndpoint();
app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

// TODO: find a better way (testing error)
public partial class Program {}