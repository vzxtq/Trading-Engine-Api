using TradingPlatform.Application.Common;
using TradingPlatform.Domain.Enums;

namespace TradingPlatform.Application.Orders.Commands;

/// <summary>
/// Command to place a new order in the system.
/// </summary>
public class PlaceOrderCommand : ICommand<PlaceOrderResponse>
{
    public Guid UserId { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Quantity { get; set; }
    public OrderSide Side { get; set; }
}

public class PlaceOrderResponse
{
    public Guid OrderId { get; set; }
    public OrderStatus Status { get; set; }
    public string Message { get; set; } = string.Empty;
}