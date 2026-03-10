using TradingPlatform.Infrastructure.Persistence.Repositories;
using TradingPlatform.Application.Interfaces;

namespace TradingPlatform.Infrastructure.Persistence.UnitOfWork;

/// <summary>
/// Unit of Work implementation for managing transactions across repositories.
/// Follows the Unit of Work pattern for data consistency.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly TradingDbContext _context;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(TradingDbContext context)
    {
        _context = context;
    }

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : Domain.Common.BaseEntity
    {
        var entityType = typeof(TEntity);

        if (!_repositories.ContainsKey(entityType))
        {
            var repositoryType = typeof(Repository<>).MakeGenericType(entityType);
            var repositoryInstance = Activator.CreateInstance(repositoryType, _context);
            _repositories.Add(entityType, repositoryInstance!);
        }

        return (IRepository<TEntity>)_repositories[entityType];
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}