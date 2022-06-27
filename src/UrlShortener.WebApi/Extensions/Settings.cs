﻿using UrlShortener.WebApi.Models.Settings;

namespace UrlShortener.WebApi.Extensions;

public static class Settings
{
    public static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
        services.Configure<AuthSettings>(configuration.GetSection(nameof(AuthSettings)));
        services.Configure<SmtpSettings>(configuration.GetSection(nameof(SmtpSettings)));
    }
}