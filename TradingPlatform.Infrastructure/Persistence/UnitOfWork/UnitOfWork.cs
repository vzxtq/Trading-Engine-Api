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

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}