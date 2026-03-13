using TradingPlatform.Domain.Entities;

namespace TradingPlatform.Domain.Repositories;

/// <summary>
/// Repository interface for Trade entity.
/// </summary>
public interface ITradeRepository
{
    Task<TradeDomain?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(TradeDomain trade, CancellationToken cancellationToken = default);

    Task<IEnumerable<TradeDomain>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<IEnumerable<TradeDomain>> GetBySymbolAsync(string symbol, CancellationToken cancellationToken = default);

    Task<IEnumerable<TradeDomain>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);

    Task<IEnumerable<TradeDomain>> GetAllAsync(CancellationToken cancellationToken = default);
}