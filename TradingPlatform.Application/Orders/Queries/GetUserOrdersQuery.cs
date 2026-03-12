using TradingPlatform.Application.Common;
using TradingPlatform.Application.DTOs;

namespace TradingPlatform.Application.Orders.Queries;

/// <summary>
/// Query to retrieve all orders for a specific user.
/// </summary>
public class GetUserOrdersQuery : IQuery<IEnumerable<OrderDto>>
{
    public Guid UserId { get; set; }
}
