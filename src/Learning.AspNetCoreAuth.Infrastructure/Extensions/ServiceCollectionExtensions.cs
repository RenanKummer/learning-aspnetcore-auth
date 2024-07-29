using Learning.AspNetCoreAuth.Core.Repositories;
using Learning.AspNetCoreAuth.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Learning.AspNetCoreAuth.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers services provided by Infrastructure layer in the service collection.
    /// </summary>
    /// <param name="services">The services instance.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services) => services
        .AddSingleton<IGameRepository, LocalGameRepository>()
        .AddSingleton<IUserRepository, LocalUserRepository>();
}