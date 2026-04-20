using Microsoft.EntityFrameworkCore;
using TradingEngine.Application.Features.Orders.Dtos;
using TradingEngine.Application.Features.Orders.Repositories;
using TradingEngine.Infrastructure.Persistence;
using TradingEngine.Application.Features.Orders.Dtos;
using TradingEngine.Domain.ValueObjects;

namespace TradingEngine.Infrastructure.Repositories.Orders;

public sealed class OrderBookReadRepository : IOrderBookReadRepository
{
    private readonly TradingDbContext _dbContext;

    public OrderBookReadRepository(TradingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OrderBookDto> GetOrderBookAsync(Symbol symbol, CancellationToken cancellationToken)
    {
        // naive projection; for production consider materialized views or cache
        var buyOrders = await _dbContext.Orders
            .Where(o => o.Symbol.Value == symbol.Value && o.Side == Domain.Enums.OrderSide.Buy)
            .OrderByDescending(o => o.Price.Value)
            .ThenBy(o => o.CreatedAt)
            .Select(o => MapOrder(o))
            .ToListAsync(cancellationToken);

        var sellOrders = await _dbContext.Orders
            .Where(o => o.Symbol.Value == symbol.Value && o.Side == Domain.Enums.OrderSide.Sell)
            .OrderBy(o => o.Price.Value)
            .ThenBy(o => o.CreatedAt)
            .Select(o => MapOrder(o))
            .ToListAsync(cancellationToken);

        return new OrderBookDto
        {
            Symbol = symbol.Value,
            BuyOrders = buyOrders,
            SellOrders = sellOrders
        };
    }

    private static OrderDto MapOrder(Domain.Entities.OrderDomain o) => new()
    {
        Id = o.Id,
        UserId = o.UserId,
        Symbol = o.Symbol.Value,
        Price = o.Price.Value,
        Quantity = o.Quantity.Value,
        RemainingQuantity = o.RemainingQuantity.Value,
        Side = o.Side,
        Status = o.Status,
        CreatedAt = o.CreatedAt,
        UpdatedAt = o.UpdatedAt
    };
}
