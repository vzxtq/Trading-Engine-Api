using TradingPlatform.Domain.Entities;

namespace TradingPlatform.Domain.Repositories;

/// <summary>
/// Repository interface for Trade entity.
/// </summary>
public interface ITradeRepository
{
    Task<Trade?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(Trade trade, CancellationToken cancellationToken = default);

    Task<IEnumerable<Trade>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Trade>> GetBySymbolAsync(string symbol, CancellationToken cancellationToken = default);

    Task<IEnumerable<Trade>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Trade>> GetAllAsync(CancellationToken cancellationToken = default);
}