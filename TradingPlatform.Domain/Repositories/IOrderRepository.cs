using TradingPlatform.Domain.Entities;
using TradingPlatform.Domain.Enums;

namespace TradingPlatform.Domain.Repositories;

/// <summary>
/// Repository interface for Order aggregate root.
/// </summary>
public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(Order order, CancellationToken cancellationToken = default);

    Task UpdateAsync(Order order, CancellationToken cancellationToken = default);

    Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default);

    Task<IEnumerable<Order>> GetPendingOrdersAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Order>> GetBySymbolAsync(string symbol, CancellationToken cancellationToken = default);
}