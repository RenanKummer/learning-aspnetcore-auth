using Learning.AspNetCoreAuth.Core.Models.Options;
using Learning.AspNetCoreAuth.WebApi.Formatters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
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
    public static WebApplicationBuilder ConfigureOptions(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configs = builder.Configuration;

        services.Configure<IdentityProviderOptions>(configs.GetRequiredSection("IdentityProviders"));
        
        return builder;
    }

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

    /// <summary>
    /// Configures authentication and authorization services.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <returns>The <see cref="WebApplicationBuilder"/> so that additional calls can be chained.</returns>
    public static WebApplicationBuilder ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        var identityProviderOptions = 
            builder.Configuration.GetRequiredSection("IdentityProviders").Get<IdentityProviderOptions>();
        
        if (identityProviderOptions?.Google is null)
            Log.Warning("Google Identity Provider settings not found in this environment");
        
        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogle(options =>
            {
                options.ClientId = identityProviderOptions?.Google?.ClientId ?? string.Empty;
                options.ClientSecret = identityProviderOptions?.Google?.ClientPassword ?? string.Empty;
            });

        return builder;
    }
}