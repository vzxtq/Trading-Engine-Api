using TradingPlatform.Domain.Enums;
using TradingPlatform.Domain.ValueObjects;

namespace TradingPlatform.Domain.Events.Orders;

/// <summary>
/// Raised when an order has been placed in the system.
/// </summary>
public class OrderPlacedEvent : DomainEvent
{
    public Guid UserId { get; }
    public Symbol Symbol { get; }
    public Price Price { get; }
    public Quantity Quantity { get; }
    public OrderSide Side { get; }

    public OrderPlacedEvent(Guid orderId, Guid userId, Symbol symbol, Price price, Quantity quantity, OrderSide side)
    {
        AggregateId = orderId;
        UserId = userId;
        Symbol = symbol;
        Price = price;
        Quantity = quantity;
        Side = side;
    }
}