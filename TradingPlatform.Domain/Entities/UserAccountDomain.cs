using TradingPlatform.Domain.Common;
using TradingPlatform.Domain.ValueObjects;

namespace TradingPlatform.Domain.Entities;

/// <summary>
/// Represents a user trading account.
/// Aggregate root responsible for balance management.
/// </summary>
public class UserAccountDomain : AggregateRoot
{
    public string Email { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public Money Balance { get; private set; } = Money.Zero();
    public Money ReservedBalance { get; private set; } = Money.Zero();
    public DateTime? LastLoginAt { get; private set; }
    public bool IsActive { get; private set; } = false;

    private UserAccountDomain() { }

    public static UserAccountDomain Create(string email, string name, Money initialBalance)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.", nameof(email));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));

        if (initialBalance.Amount < 0)
            throw new ArgumentException("Initial balance cannot be negative.", nameof(initialBalance));

        return new UserAccountDomain
        {
            Id = Guid.NewGuid(),
            Email = email.Trim().ToLowerInvariant(),
            Name = name.Trim(),
            Balance = initialBalance,
            ReservedBalance = Money.Zero(initialBalance.Currency),
            CreatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Returns funds available for trading.
    /// </summary>
    public Money AvailableBalance => Balance - ReservedBalance;

    /// <summary>
    /// Deposits funds into the account.
    /// </summary>
    public void Deposit(Money amount)
    {
        EnsureActive();
        EnsurePositive(amount);

        Balance += amount;

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Withdraws funds from the account.
    /// </summary>
    public void Withdraw(Money amount)
    {
        EnsureActive();
        EnsurePositive(amount);

        if (AvailableBalance < amount)
            throw new InvalidOperationException("Insufficient available balance.");

        Balance -= amount;

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Reserves funds for an order.
    /// </summary>
    public void ReserveFunds(Money amount)
    {
        EnsureActive();
        EnsurePositive(amount);

        if (AvailableBalance < amount)
            throw new InvalidOperationException("Insufficient funds for reservation.");

        ReservedBalance += amount;

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Releases reserved funds when an order is cancelled.
    /// </summary>
    public void ReleaseReservedFunds(Money amount)
    {
        EnsurePositive(amount);

        if (ReservedBalance < amount)
            throw new InvalidOperationException("Cannot release more than reserved.");

        ReservedBalance -= amount;

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Finalizes a trade and deducts reserved funds.
    /// </summary>
    public void CommitReservedFunds(Money amount)
    {
        EnsurePositive(amount);

        if (ReservedBalance < amount)
            throw new InvalidOperationException("Reserved balance is insufficient.");

        ReservedBalance -= amount;
        Balance -= amount;

        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    private void EnsureActive()
    {
        if (!IsActive)
            throw new InvalidOperationException("Account is inactive.");
    }

    private static void EnsurePositive(Money amount)
    {
        if (amount.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.");
    }
}