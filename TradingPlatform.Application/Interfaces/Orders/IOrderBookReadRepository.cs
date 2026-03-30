using TradingPlatform.Application.Features.Orders.Dtos;
using TradingPlatform.Domain.ValueObjects;

namespace TradingEngine.Application.Features.Orders.Repositories;

public interface IOrderBookReadRepository
{
    Task<OrderBookDto> GetOrderBookAsync(Symbol symbol, CancellationToken cancellationToken);
}
