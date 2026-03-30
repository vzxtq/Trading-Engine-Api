using TradingEngine.Application.Common;
using TradingEngine.Application.Features.Orders.Repositories;
using TradingEngine.Application.Interfaces.Orders;
using TradingPlatform.Application.DTOs;

namespace TradingEngine.Application.Features.Orders.Queries;


public class GetUserOrdersQuery : IQuery<IReadOnlyList<OrderDto>>
{
    public Guid UserId { get; set; }
}

public sealed class GetUserOrdersQueryHandler : IQueryHandler<GetUserOrdersQuery, IReadOnlyList<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetUserOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    public async Task<IReadOnlyList<OrderDto>> Handle(GetUserOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetUserOrdersAsync(request.UserId, cancellationToken);

        return orders
            .Select(order => new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                Symbol = order.Symbol.Value,
                Price = order.Price.Value,
                Quantity = order.Quantity.Value,
                RemainingQuantity = order.RemainingQuantity.Value,
                Side = order.Side,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt
            })
            .ToList();
    }
}
