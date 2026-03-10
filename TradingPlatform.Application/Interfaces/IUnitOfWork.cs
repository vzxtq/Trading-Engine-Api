namespace TradingPlatform.Application.Interfaces;

/// <summary>
/// Unit of Work pattern interface for managing transactions across multiple repositories.
/// Ensures data consistency in operations involving multiple entities.
/// </summary>
public interface IUnitOfWork : IAsyncDisposable
{
    /// <summary>
    /// Gets the repository for the specified entity type.
    /// </summary>
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : Domain.Common.BaseEntity;

    /// <summary>
    /// Commits all changes made in the current unit of work asynchronously.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
