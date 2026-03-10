using Microsoft.EntityFrameworkCore;

namespace TradingPlatform.Infrastructure.Persistence;

/// <summary>
/// Main database context for the Trading Platform application.
/// Manages all entity mappings and database operations using Entity Framework Core.
/// Follows DDD and clean architecture principles.
/// </summary>
public class TradingDbContext : DbContext
{
    public TradingDbContext(DbContextOptions<TradingDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Configures model mappings using entity type configurations from the assembly.
    /// This allows centralized configuration of entities using IEntityTypeConfiguration pattern.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity type configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TradingDbContext).Assembly);
    }
}