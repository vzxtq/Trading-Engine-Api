using Microsoft.EntityFrameworkCore;
using TradingPlatform.Application.Interfaces;
using TradingPlatform.Domain.Entities;
using TradingPlatform.Domain.ValueObjects;
using TradingPlatform.Infrastructure.Persistence;

namespace TradingPlatform.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly TradingDbContext _dbContext;

    public AccountRepository(TradingDbContext context)
    {
        _dbContext = context;
    }

    public async Task AddAsync(
        UserAccountDomain account,
        CancellationToken cancellationToken)
    {
        var entity = MapToEntity(account);

        await _dbContext.Accounts.AddAsync(entity, cancellationToken);
    }

    public async Task<UserAccountDomain?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? null : MapToDomain(entity);
    }

    public Task UpdateAsync(
        UserAccountDomain account,
        CancellationToken cancellationToken)
    {
        var entity = MapToEntity(account);

        _dbContext.Accounts.Update(entity);

        return Task.CompletedTask;
    }


    private static UserAccount MapToEntity(UserAccountDomain domain)
    {
        return new UserAccount
        {
            Id = domain.Id,
            Email = domain.Email,
            Name = domain.Name,
            Balance = domain.Balance,
            ReservedBalance = domain.ReservedBalance,
            CreatedAt = domain.CreatedAt,
            UpdatedAt = domain.UpdatedAt
        };
    }

    private static UserAccountDomain MapToDomain(UserAccount entity)
    {
        return UserAccountDomain.Create(
            entity.Email,
            entity.Name,
            new Money(entity.Balance.Amount, entity.Balance.Currency)
        );
    }
}
