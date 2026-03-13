namespace TradingPlatform.Domain.ValueObjects;

/// <summary>
/// Represents a monetary value with currency.
/// Immutable value object.
/// </summary>
public sealed class Money : IEquatable<Money>
{
    public decimal Amount { get; }

    public string Currency { get; }

    public Money()
    {
        Currency = "USD";
    }

    public Money(decimal amount, string currency = "USD")
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty.", nameof(currency));

        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    public static Money Zero(string currency = "USD") => new(0, currency);

    private static void EnsureSameCurrency(Money a, Money b)
    {
        if (!string.Equals(a.Currency, b.Currency, StringComparison.Ordinal))
            throw new InvalidOperationException(
                $"Currency mismatch: {a.Currency} vs {b.Currency}");
    }

    public static Money operator +(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public static Money operator -(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return new Money(a.Amount - b.Amount, a.Currency);
    }

    public static bool operator >(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return a.Amount > b.Amount;
    }

    public static bool operator <(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return a.Amount < b.Amount;
    }

    public static bool operator >=(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return a.Amount >= b.Amount;
    }

    public static bool operator <=(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return a.Amount <= b.Amount;
    }

    public bool Equals(Money? other)
    {
        if (other is null)
            return false;

        return Amount == other.Amount &&
               string.Equals(Currency, other.Currency, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj) => Equals(obj as Money);

    public override int GetHashCode()
        => HashCode.Combine(Amount, Currency);

    public override string ToString()
        => $"{Amount:F2} {Currency}";
}