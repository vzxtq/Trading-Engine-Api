namespace TradingPlatform.Domain.Enums;

/// <summary>
/// Represents the status of an order throughout its lifecycle.
/// </summary>
public enum OrderStatus
{
    /// <summary>Order is pending execution.</summary>
    Pending = 1,

    /// <summary>Order has been partially filled.</summary>
    PartiallyFilled = 2,

    /// <summary>Order has been fully filled.</summary>
    Filled = 3,

    /// <summary>Order has been cancelled by the user.</summary>
    Cancelled = 4,

    /// <summary>Order has been rejected by the matching engine.</summary>
    Rejected = 5
}
