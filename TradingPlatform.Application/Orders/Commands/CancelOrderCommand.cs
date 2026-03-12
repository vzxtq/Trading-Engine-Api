using TradingPlatform.Application.Common;

namespace TradingPlatform.Application.Orders.Commands;

/// <summary>
/// Command to cancel an existing order.
/// </summary>
public class CancelOrderCommand : ICommand<CancelOrderResponse>
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
}

public class CancelOrderResponse
{
    public Guid OrderId { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}