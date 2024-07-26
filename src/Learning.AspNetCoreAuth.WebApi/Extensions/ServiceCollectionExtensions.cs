using System.Text.Json;
using System.Text.Json.Serialization;
using Learning.AspNetCoreAuth.WebApi.Middlewares;
using NSwag;

namespace Learning.AspNetCoreAuth.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers services provided by Web API layer in the service collection.
    /// </summary>
    /// <param name="services">The services instance.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.AllowTrailingCommas = true;
                options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
            });

        return services
            .AddOpenApi()
            .AddProblemDetails()
            .AddExceptionHandler<GlobalExceptionHandler>();
    }

    private static IServiceCollection AddOpenApi(this IServiceCollection services)
    {
        return services.AddOpenApiDocument((settings, _) => settings.PostProcess = document =>
        {
            document.Info = new OpenApiInfo
            {
                Title = "Learning Series: ASP.NET Core Authentication & Authorization",
                Description = "Explores how to configure authentication and authorization in ASP.NET Core.",
                Contact = new OpenApiContact { Name = "@RenanKummer", Url = "https://github.com/RenanKummer" }
            };
        });
    }
}