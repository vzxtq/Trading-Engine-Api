using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TradingPlatform.Application.Interfaces;
using TradingPlatform.Infrastructure.Persistence;
using TradingPlatform.Infrastructure.Persistence.Repositories;
using TradingPlatform.Infrastructure.Persistence.UnitOfWork;

namespace TradingPlatform.Infrastructure;

/// <summary>
/// Extension method for registering Infrastructure layer services.
/// Configures Entity Framework Core, repositories, and Unit of Work pattern.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Infrastructure layer services to the dependency injection container.
    /// Configures DbContext, repositories, and Unit of Work for data access.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">Application configuration for connection strings</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register DbContext with SQL Server
        services.AddDbContext<TradingDbContext>(options =>
         options.UseSqlServer(
            configuration.GetConnectionString("DefaultConnection"),
      sqlOptions => sqlOptions.MigrationsAssembly("TradingPlatform.Infrastructure")));

        // Register Unit of Work pattern for transaction management
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register generic repository - can be used directly or through Unit of Work
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
}