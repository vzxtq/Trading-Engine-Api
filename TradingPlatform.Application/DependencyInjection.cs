using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TradingPlatform.Application.Interfaces;

namespace TradingPlatform.Application;

/// <summary>
/// Extension method for registering Application layer services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Application layer services to the dependency injection container.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}
