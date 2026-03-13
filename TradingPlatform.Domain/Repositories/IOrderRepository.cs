using TradingPlatform.Domain.Entities;
using TradingPlatform.Domain.Enums;

namespace TradingPlatform.Domain.Repositories;

/// <summary>
/// Repository interface for Order aggregate root.
/// </summary>
public interface IOrderRepository
{
    Task<OrderDomain?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(OrderDomain order, CancellationToken cancellationToken = default);

    Task UpdateAsync(OrderDomain order, CancellationToken cancellationToken = default);

    Task<IEnumerable<OrderDomain>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<IEnumerable<OrderDomain>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default);

    Task<IEnumerable<OrderDomain>> GetPendingOrdersAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<OrderDomain>> GetBySymbolAsync(string symbol, CancellationToken cancellationToken = default);
}