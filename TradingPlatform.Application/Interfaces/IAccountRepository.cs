using TradingPlatform.Domain.Entities;

namespace TradingPlatform.Application.Interfaces
{
    public interface IAccountRepository
    {
        Task<UserAccountDomain?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken);
        Task AddAsync(
            UserAccountDomain account,
            CancellationToken cancellationToken);
        Task UpdateAsync(
            UserAccountDomain account,
            CancellationToken cancellationToken);
    }
}
