using TradingPlatform.Domain.Enums;

namespace TradingPlatform.Application.Features.Orders.Dtos;

public class PlaceOrderResponseDto
{
    public Guid OrderId { get; set; }
    public OrderStatus Status { get; set; }
    public string Message { get; set; } = string.Empty;
}
