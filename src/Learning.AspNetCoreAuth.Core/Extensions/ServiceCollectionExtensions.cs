using Microsoft.Extensions.DependencyInjection;

namespace Learning.AspNetCoreAuth.Core.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers services provided by Core layer in the service collection.
    /// </summary>
    /// <param name="services">The services instance.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddCoreServices(this IServiceCollection services) => services;
}