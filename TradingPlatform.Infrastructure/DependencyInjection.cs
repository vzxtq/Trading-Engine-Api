using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TradingEngine.Infrastructure.Repositories.Accounts;
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
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAccountRepository, AccountRepository>();

        return services;
    }
}