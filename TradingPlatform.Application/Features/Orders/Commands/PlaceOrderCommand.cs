using TradingEngine.Application.Features.Orders.Repositories;
using TradingEngine.Application.Interfaces.Orders;
using TradingEngine.Domain.Entities;
using TradingEngine.Domain.Enums;
using TradingEngine.MatchingEngine.Abstractions;
using TradingEngine.MatchingEngine.Commands;
using TradingPlatform.Application.Common;
using TradingPlatform.Application.Features.Orders.Dtos;
using TradingPlatform.Domain.ValueObjects;

namespace TradingEngine.Application.Features.Orders.Commands;

/// <summary>
/// Command to place a new order in the system.
/// </summary>
public class PlaceOrderCommand : ICommand<Result<PlaceOrderResponseDto>>
{
    public Guid UserId { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public long Price { get; set; }
    public long Quantity { get; set; }
    public OrderSide Side { get; set; }
}

public sealed class PlaceOrderCommandHandler : ICommandHandler<PlaceOrderCommand, Result<PlaceOrderResponseDto>>
{
    private readonly IMatchingEngineQueue _queue;
    private readonly IOrderRepository _orderRepository;

    public PlaceOrderCommandHandler(
        IMatchingEngineQueue queue,
        IOrderRepository orderRepository)
    {
        _queue = queue;
        _orderRepository = orderRepository;
    }

    public async Task<Result<PlaceOrderResponseDto>> Handle(
        PlaceOrderCommand request,
        CancellationToken cancellationToken)
    {
        var orderId = Guid.NewGuid();

        try
        {
            var symbol = new Symbol(request.Symbol);
            var price = new Price(request.Price);
            var quantity = new Quantity(request.Quantity);

            var order = OrderDomain.Create(
                request.UserId,
                symbol,
                price,
                quantity,
                request.Side);

            await _orderRepository.AddAsync(order, cancellationToken);

            var command = new AddOrderCommand
            {
                OrderId = order.Id,
                UserId = request.UserId,
                Symbol = symbol,
                Price = request.Price,
                Quantity = request.Quantity,
                Side = request.Side,
                ReceivedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            await _queue.EnqueueAsync(command, cancellationToken);

            return Result<PlaceOrderResponseDto>.Success(new PlaceOrderResponseDto
            {
                OrderId = order.Id,
                Status = OrderStatus.Open,
                Message = "Order queued for matching"
            });
        }
        catch (Exception ex)
        {
            return Result<PlaceOrderResponseDto>.Failure(
                $"Failed to place order: {ex.Message}");
        }
    }
}
