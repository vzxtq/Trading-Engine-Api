using TradingEngine.MatchingEngine.Models;
using TradingPlatform.Domain.ValueObjects;

namespace TradingEngine.MatchingEngine.Abstractions;

public interface IOrderBookSnapshotProvider
{
    Task<OrderBookSnapshot> GetSnapshotAsync(Symbol symbol, CancellationToken cancellationToken);
}
