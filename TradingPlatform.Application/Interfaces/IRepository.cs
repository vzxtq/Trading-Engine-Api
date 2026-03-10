using TradingPlatform.Domain.Common;

namespace TradingPlatform.Application.Interfaces;

/// <summary>
/// Generic repository interface for data access operations.
/// Defines contract for all repositories to follow.
/// </summary>
/// <typeparam name="TEntity">The entity type managed by this repository</typeparam>
public interface IRepository<TEntity> where TEntity : BaseEntity
{
    /// <summary>
    /// Gets all entities asynchronously.
 /// </summary>
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an entity by its ID asynchronously.
    /// </summary>
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new entity asynchronously.
    /// </summary>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity asynchronously.
    /// </summary>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes an entity asynchronously.
    /// </summary>
    Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves changes to the database asynchronously.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
