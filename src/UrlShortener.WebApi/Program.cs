using UrlShortener.WebApi.Extensions.Startup;

namespace UrlShortener.WebApi;

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