using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingPlatform.Infrastructure.Persistence;

namespace TradingPlatform.Infrastructure.Persistence.Configurations;

public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        builder.ToTable("Accounts");

        builder.HasKey(x => x.Id)
               .HasName("PK_User_Accounts");

        builder.HasIndex(x => x.Email)
               .IsUnique()
               .HasDatabaseName("UX_User_Accounts_Email");

        builder.HasIndex(x => x.IsActive)
               .HasDatabaseName("IX_User_Accounts_Is_Active");

        builder.Property(x => x.Email)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(150);

        builder.OwnsOne(x => x.Balance, balance =>
        {
            balance.Property(m => m.Amount)
                   .HasColumnName("BalanceAmount")
                   .HasPrecision(18, 8);
            balance.Property(m => m.Currency)
                   .HasColumnName("BalanceCurrency")
                   .HasMaxLength(3);
        });

        builder.OwnsOne(x => x.ReservedBalance, reserved =>
        {
            reserved.Property(m => m.Amount)
                    .HasColumnName("ReservedBalanceAmount")
                    .HasPrecision(18, 8);
            reserved.Property(m => m.Currency)
                   .HasColumnName("ReservedBalanceCurrency")
                   .HasMaxLength(3);
        });

        builder.Property(x => x.LastLoginAt)
               .HasColumnName("LastLoginAt");

        builder.Property(x => x.IsActive)
               .HasColumnName("IsActive");

        builder.Property(x => x.CreatedAt)
               .HasColumnName("CreatedAt");

        builder.Property(x => x.UpdatedAt)
               .HasColumnName("UpdatedAt");
    }
}