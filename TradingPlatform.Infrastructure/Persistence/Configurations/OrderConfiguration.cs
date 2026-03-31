using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingEngine.Domain.Entities;

namespace TradingPlatform.Infrastructure.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<OrderDomain>
{
    public void Configure(EntityTypeBuilder<OrderDomain> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id)
            .HasName("PK_Orders");

        builder.HasIndex(o => new { o.UserId, o.CreatedAt })
            .HasDatabaseName("IX_Orders_User_CreatedAt");

        builder.Property(o => o.UserId)
            .IsRequired();

        builder.Property(o => o.Side)
            .IsRequired();

        builder.Property(o => o.Status)
            .IsRequired();

        builder.OwnsOne(o => o.Symbol, sym =>
        {
            sym.Property(s => s.Value)
               .HasColumnName("Symbol")
               .HasMaxLength(10)
               .IsRequired();
        });

        builder.OwnsOne(o => o.Price, price =>
        {
            price.Property(p => p.Value)
                 .HasColumnName("Price")
                 .HasPrecision(18, 8)
                 .IsRequired();
        });

        builder.OwnsOne(o => o.Quantity, qty =>
        {
            qty.Property(q => q.Value)
               .HasColumnName("Quantity")
               .HasPrecision(18, 8)
               .IsRequired();
        });

        builder.OwnsOne(o => o.RemainingQuantity, qty =>
        {
            qty.Property(q => q.Value)
               .HasColumnName("RemainingQuantity")
               .HasPrecision(18, 8)
               .IsRequired();
        });

        builder.Property(o => o.CreatedAt).IsRequired();
        builder.Property(o => o.UpdatedAt);
    }
}
