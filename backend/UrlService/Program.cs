using FluentValidation.AspNetCore;
using UrlService.Database;
using UrlService.Startup;

var builder = WebApplication.CreateBuilder(args);

// Program
var allOptions = builder.GetAllOptions();
builder.WebHost.AddAwsParameterStore();
builder.WebHost.AddSerilog(builder.Environment, allOptions.LoggingOptions);

// Configure services
builder.Services.ConfigureOptions(builder.Configuration, builder.Environment);
builder.Services.ConfigureDbContext(allOptions.ConnectionOptions);
builder.Services.AddCache(allOptions.ConnectionOptions);
builder.Services.AddValidators();
builder.Services.RegisterServices(allOptions.ConnectionOptions);
builder.Services.AddAuthentication(allOptions.AuthOptions);
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwagger();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => { policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
});

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
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

// TODO: find a better way (testing error)
namespace UrlService
{
    public partial class Program {}
}