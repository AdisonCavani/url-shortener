using UrlShortener.Api.Extensions.Startup;

namespace UrlShortener.Api;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .AddSerilog()
            .AddAwsParameterStore()
#if RELEASE
            .AddAppMetrics()
#endif
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}