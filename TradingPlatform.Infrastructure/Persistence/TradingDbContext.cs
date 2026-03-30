using Microsoft.EntityFrameworkCore;
using TradingPlatform.Domain.Entities;
using TradingPlatform.Infrastructure.Persistence.Configurations;

namespace TradingPlatform.Infrastructure.Persistence;

public class TradingDbContext : DbContext
{
    public TradingDbContext(DbContextOptions<TradingDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserAccountDomain> UserAccounts => Set<UserAccountDomain>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply configurations manually instead of using reflection
        modelBuilder.ApplyConfiguration(new UserAccountConfiguration());
    }
}
