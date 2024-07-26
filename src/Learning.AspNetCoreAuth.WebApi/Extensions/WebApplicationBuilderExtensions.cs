using Learning.AspNetCoreAuth.WebApi.Formatters;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;

namespace Learning.AspNetCoreAuth.WebApi.Extensions;

public static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Registers configurations from <c>appsettings.{Environment}.json</c> in the service collection.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <returns>The <see cref="WebApplicationBuilder"/> so that additional calls can be chained.</returns>
    public static WebApplicationBuilder ConfigureOptions(this WebApplicationBuilder builder) => builder;

    /// <summary>
    /// Configures the logging provider.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <returns>The <see cref="WebApplicationBuilder"/> so that additional calls can be chained.</returns>
    public static WebApplicationBuilder ConfigureLoggingProvider(this WebApplicationBuilder builder)
    {
        builder.Services.AddSerilog((services, logConfigs) =>
        {
            logConfigs
                .ReadFrom.Configuration(builder.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .Enrich.WithApplicationName()
                .Enrich.WithExceptionDetails()
                .WriteTo.Async(sink => sink.Console(new CompactJsonFormatter(new SerilogNullJsonValueFormatter())));
        });

        return builder;
    }
}