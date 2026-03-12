using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TradingPlatform.Application.Interfaces;
using TradingPlatform.Infrastructure.Persistence;
using TradingPlatform.Infrastructure.Persistence.UnitOfWork;

namespace TradingPlatform.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TradingDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            sqlOptions => sqlOptions.MigrationsAssembly("TradingPlatform.Infrastructure")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}