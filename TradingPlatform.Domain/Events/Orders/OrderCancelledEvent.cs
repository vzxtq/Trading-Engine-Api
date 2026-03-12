using TradingPlatform.Domain.Common;

namespace TradingPlatform.Domain.Events.Orders;

/// <summary>
/// Raised when an order is cancelled.
/// </summary>
public class OrderCancelledEvent : DomainEvent
{
    public Guid OrderId { get; }

    public OrderCancelledEvent(Guid orderId)
    {
        AggregateId = orderId;
        OrderId = orderId;
    }
}