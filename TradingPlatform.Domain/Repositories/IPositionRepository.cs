using TradingPlatform.Domain.Entities;

namespace TradingPlatform.Domain.Repositories;

/// <summary>
/// Repository interface for Position entity.
/// </summary>
public interface IPositionRepository
{
    Task<PositionDomain?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<PositionDomain?> GetByUserAndSymbolAsync(Guid userId, string symbol, CancellationToken cancellationToken = default);

    Task AddAsync(PositionDomain position, CancellationToken cancellationToken = default);

    Task UpdateAsync(PositionDomain position, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<PositionDomain>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}