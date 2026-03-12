using TradingPlatform.Domain.Entities;

namespace TradingPlatform.Domain.Repositories;

/// <summary>
/// Repository interface for UserAccount aggregate root.
/// </summary>
public interface IUserAccountRepository
{
    Task<UserAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<UserAccount?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task AddAsync(UserAccount account, CancellationToken cancellationToken = default);

    Task UpdateAsync(UserAccount account, CancellationToken cancellationToken = default);

    Task<IEnumerable<UserAccount>> GetAllAsync(CancellationToken cancellationToken = default);
}