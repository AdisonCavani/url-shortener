using FluentValidation.AspNetCore;
using ManagementService.Database;
using ManagementService.Startup;

var builder = WebApplication.CreateBuilder(args);

// Program
var allOptions = builder.GetAllOptions();
builder.WebHost.AddAwsParameterStore();
builder.WebHost.AddSerilog(builder.Environment);

// Configure services
builder.Services.ConfigureOptions(builder.Configuration, builder.Environment);
builder.Services.ConfigureDbContext(allOptions.ConnectionOptions);
builder.Services.AddValidators();
builder.Services.RegisterServices();
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwagger();

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
app.UseAuthorization();
app.MapControllers();
app.Run();