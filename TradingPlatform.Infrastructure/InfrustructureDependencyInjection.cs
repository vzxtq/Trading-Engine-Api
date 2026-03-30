using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TradingEngine.Infrastructure.Repositories.Accounts;
using TradingEngine.Application.Interfaces.Accounts;
using TradingPlatform.Infrastructure.Persistence;

namespace TradingPlatform.Infrastructure;

public static class InfrustructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IAccountReadRepository, AccountReadRepository>();

        return services;
    }
}