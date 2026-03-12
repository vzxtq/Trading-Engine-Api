using TradingPlatform.Application.Common;
using TradingPlatform.Application.DTOs;

namespace TradingPlatform.Application.Orders.Queries;

/// <summary>
/// Query to retrieve an order by its ID.
/// </summary>
public class GetOrderByIdQuery : IQuery<OrderDto?>
{
   public Guid OrderId { get; set; }
}
