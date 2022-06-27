﻿using Microsoft.EntityFrameworkCore;
using UrlShortener.WebApi.Models.App;

namespace UrlShortener.WebApi.Extensions;

public static class DbContext
{
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration["AppSettings:SqlConnection"], sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
            }));
    }
}