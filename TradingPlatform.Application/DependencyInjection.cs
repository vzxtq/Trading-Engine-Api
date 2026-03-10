using Microsoft.Extensions.DependencyInjection;

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
     // MediatR would be registered here for CQRS pattern
        // services.AddMediatR(typeof(DependencyInjection).Assembly);

  return services;
    }
}
