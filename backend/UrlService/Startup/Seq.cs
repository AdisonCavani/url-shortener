using Serilog;
using UrlService.Options;

namespace UrlService.Startup;

public static class Seq
{
    public static void UseSeq(this LoggerConfiguration configuration, LoggingOptions loggingOptions)
    {
        configuration.WriteTo.Seq(loggingOptions.SeqConnectionString);
    }
}