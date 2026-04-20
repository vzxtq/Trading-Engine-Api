using Microsoft.EntityFrameworkCore;
using TradingEngine.Domain.Entities;
using TradingEngine.Domain.Entities;
using TradingEngine.Domain.Events;
using TradingEngine.Infrastructure.Persistence.Configurations;

namespace TradingEngine.Infrastructure.Persistence
{
    public class TradingDbContext : DbContext
    {
        public TradingDbContext(DbContextOptions<TradingDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserAccountDomain> UserAccounts => Set<UserAccountDomain>();
        public DbSet<UserIdentityDomain> UserIdentities => Set<UserIdentityDomain>();
        public DbSet<OrderDomain> Orders => Set<OrderDomain>();
        public DbSet<TradeDomain> Trades => Set<TradeDomain>();
        public DbSet<PositionDomain> Positions => Set<PositionDomain>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<DomainEvent>();

            modelBuilder.ApplyConfiguration(new UserAccountConfiguration());
            modelBuilder.ApplyConfiguration(new UserIdentityConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new TradeConfiguration());
            modelBuilder.ApplyConfiguration(new PositionConfiguration());
        }
    }
}
