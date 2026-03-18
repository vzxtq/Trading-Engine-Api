using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TradingEngine.Application.Accounts.Dtos;
using TradingEngine.Application.Interfaces.Accounts;
using TradingPlatform.Infrastructure.Persistence;

namespace TradingEngine.Infrastructure.Repositories.Accounts
{
    public class AccountReadRepository : IAccountReadRepository
    {
        private readonly TradingDbContext _dbContext;

        public AccountReadRepository(TradingDbContext context)
        {
            _dbContext = context;
        }

        public async Task<AccountViewDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Accounts.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (entity == null)
                return null;

            return new AccountViewDto
            {
                Email = entity.Email,
                Name = entity.Name,
                Balance = entity.Balance,
                ReservedBalance = entity.ReservedBalance,
                LastLoginAt = entity.LastLoginAt,
                IsActive = entity.IsActive
            };
        }
    }
}
