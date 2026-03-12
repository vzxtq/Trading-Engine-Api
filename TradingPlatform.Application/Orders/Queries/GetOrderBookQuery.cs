using TradingPlatform.Application.Common;
using TradingPlatform.Application.DTOs;

namespace TradingPlatform.Application.Orders.Queries;

/// <summary>
/// Query to retrieve the order book for a specific symbol.
/// </summary>
public class GetOrderBookQuery : IQuery<OrderBookDto>
{
    public string Symbol { get; set; } = string.Empty;
}

public class OrderBookDto
{
    public string Symbol { get; set; } = string.Empty;
    public List<OrderDto> BuyOrders { get; set; } = new();
    public List<OrderDto> SellOrders { get; set; } = new();
}