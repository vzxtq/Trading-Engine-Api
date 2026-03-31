using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingPlatform.Domain.Entities;

namespace TradingPlatform.Infrastructure.Persistence.Configurations;

public sealed class TradeConfiguration : IEntityTypeConfiguration<TradeDomain>
{
    public void Configure(EntityTypeBuilder<TradeDomain> builder)
    {
        builder.ToTable("Trades");

        builder.HasKey(t => t.Id)
            .HasName("PK_Trades");

        builder.HasIndex(t => t.ExecutedAt)
            .HasDatabaseName("IX_Trades_ExecutedAt");

        builder.Property(t => t.BuyOrderId).IsRequired();
        builder.Property(t => t.SellOrderId).IsRequired();
        builder.Property(t => t.BuyerId).IsRequired();
        builder.Property(t => t.SellerId).IsRequired();

        builder.OwnsOne(t => t.Symbol, sym =>
        {
            sym.Property(s => s.Value)
               .HasColumnName("Symbol")
               .HasMaxLength(10)
               .IsRequired();
        });

        builder.OwnsOne(t => t.Price, price =>
        {
            price.Property(p => p.Value)
                 .HasColumnName("Price")
                 .HasPrecision(18, 8)
                 .IsRequired();
        });

        builder.OwnsOne(t => t.Quantity, qty =>
        {
            qty.Property(q => q.Value)
               .HasColumnName("Quantity")
               .HasPrecision(18, 8)
               .IsRequired();
        });

        builder.Property(t => t.ExecutedAt)
               .HasColumnName("ExecutedAt")
               .IsRequired();

        builder.Property(t => t.CreatedAt)
               .HasColumnName("CreatedAt")
               .HasDefaultValueSql("SYSUTCDATETIME()");
    }
}
