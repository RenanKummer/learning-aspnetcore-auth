using Serilog;
using Serilog.Configuration;

namespace Learning.AspNetCoreAuth.WebApi.Extensions;

public static class LoggerEnrichmentConfigurationExtensions
{
    public static LoggerConfiguration WithApplicationName(this LoggerEnrichmentConfiguration configs) => 
        configs.WithProperty("ApplicationName", "learning-aspnetcore-auth");
}