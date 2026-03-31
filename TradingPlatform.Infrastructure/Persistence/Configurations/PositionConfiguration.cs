using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingPlatform.Domain.Entities;

namespace TradingPlatform.Infrastructure.Persistence.Configurations;

public sealed class PositionConfiguration : IEntityTypeConfiguration<PositionDomain>
{
    public void Configure(EntityTypeBuilder<PositionDomain> builder)
    {
        builder.ToTable("Positions");

        builder.HasKey(p => p.Id)
               .HasName("PK_Positions");

        builder.HasIndex(p => new { p.UserId, p.Symbol })
               .IsUnique()
               .HasDatabaseName("UX_Positions_User_Symbol");

        builder.Property(p => p.UserId).IsRequired();

        builder.OwnsOne(p => p.Symbol, sym =>
        {
            sym.Property(s => s.Value)
               .HasColumnName("Symbol")
               .HasMaxLength(10)
               .IsRequired();
        });

        builder.OwnsOne(p => p.Quantity, qty =>
        {
            qty.Property(q => q.Value)
               .HasColumnName("Quantity")
               .HasPrecision(18, 8)
               .IsRequired();
        });

        builder.Property(p => p.AverageCost)
               .HasPrecision(18, 8)
               .IsRequired();

        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.UpdatedAt);
    }
}
