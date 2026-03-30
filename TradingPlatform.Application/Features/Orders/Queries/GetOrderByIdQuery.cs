using TradingEngine.Application.Features.Orders.Repositories;
using TradingEngine.Application.Interfaces.Orders;
using TradingEngine.Application.Common;
using TradingEngine.Application.Features.Orders.Dtos;

namespace TradingEngine.Application.Features.Orders.Queries;

/// <summary>
/// Query to retrieve an order by its ID.
/// </summary>
public class GetOrderByIdQuery : IQuery<OrderDto?>
{
   public Guid OrderId { get; set; }
}

public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null) return null;

        return new OrderDto
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
        };
    }
}
