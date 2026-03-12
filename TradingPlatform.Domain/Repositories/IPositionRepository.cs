using TradingPlatform.Domain.Entities;

namespace TradingPlatform.Domain.Repositories;

/// <summary>
/// Repository interface for Position entity.
/// </summary>
public interface IPositionRepository
{
    Task<Position?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Position?> GetByUserAndSymbolAsync(Guid userId, string symbol, CancellationToken cancellationToken = default);

    Task AddAsync(Position position, CancellationToken cancellationToken = default);

    Task UpdateAsync(Position position, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Position>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}