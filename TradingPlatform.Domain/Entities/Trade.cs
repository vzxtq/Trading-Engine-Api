using TradingPlatform.Domain.Common;
using TradingPlatform.Domain.ValueObjects;

namespace TradingPlatform.Domain.Entities;

/// <summary>
/// Represents an executed trade (match between a buy and sell order).
/// </summary>
public class Trade : BaseEntity
{
    public Guid BuyOrderId { get; private set; }
    public Guid SellOrderId { get; private set; }
    public Guid BuyerId { get; private set; }
    public Guid SellerId { get; private set; }
    public Symbol Symbol { get; private set; } = null!;
    public Price Price { get; private set; } = null!;
    public Quantity Quantity { get; private set; } = null!;
    public DateTime ExecutedAt { get; private set; }

    private Trade()
    { }

    /// <summary>
    /// Creates a new executed trade.
    /// </summary>
    public static Trade Create(
        Guid buyOrderId,
        Guid sellOrderId,
        Guid buyerId,
        Guid sellerId,
        Symbol symbol,
        Price price,
        Quantity quantity)
    {
        return new Trade
        {
            Id = Guid.NewGuid(),
            BuyOrderId = buyOrderId,
            SellOrderId = sellOrderId,
            BuyerId = buyerId,
            SellerId = sellerId,
            Symbol = symbol,
            Price = price,
            Quantity = quantity,
            ExecutedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Gets the total value of the trade (price * quantity).
    /// </summary>
    public decimal GetTotalValue() => Price.Value * Quantity.Value;
}