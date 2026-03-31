using Microsoft.EntityFrameworkCore;
using TradingEngine.Domain.Entities;
using TradingPlatform.Domain.Entities;
using TradingPlatform.Infrastructure.Persistence.Configurations;

namespace TradingEngine.Infrastructure.Persistence
{
    public class TradingDbContext : DbContext
    {
        public TradingDbContext(DbContextOptions<TradingDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserAccountDomain> UserAccounts => Set<UserAccountDomain>();
        public DbSet<OrderDomain> Orders => Set<OrderDomain>();
        public DbSet<TradeDomain> Trades => Set<TradeDomain>();
        public DbSet<PositionDomain> Positions => Set<PositionDomain>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserAccountConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new TradeConfiguration());
            modelBuilder.ApplyConfiguration(new PositionConfiguration());
        }
    }
}
