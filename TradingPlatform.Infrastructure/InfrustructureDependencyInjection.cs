using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TradingEngine.Application.Features.Orders.Repositories;
using TradingEngine.Application.Interfaces.Accounts;
using TradingEngine.Infrastructure.Persistence;
using TradingEngine.Infrastructure.Repositories.Accounts;
using TradingEngine.Infrastructure.Repositories.Orders;

namespace TradingPlatform.Infrastructure;

public static class InfrustructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TradingDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IAccountReadRepository, AccountReadRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderBookReadRepository, OrderBookReadRepository>();

        return services;
    }
}
