using TradingEngine.Application.Features.Orders.Repositories;
using TradingEngine.Application.Interfaces.Orders;
using TradingEngine.MatchingEngine.Abstractions;
using TradingEngine.MatchingEngine.Commands;
using TradingPlatform.Application.Common;
using TradingPlatform.Application.Features.Orders.Dtos;
using TradingPlatform.Domain.Enums;
using TradingPlatform.Domain.ValueObjects;

namespace TradingEngine.Application.Features.Orders.Commands;

/// <summary>
/// Command to cancel an existing order.
/// </summary>
public class CancelOrderCommand : ICommand<CancelOrderResponseDto>
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
}

public sealed class CancelOrderCommandHandler : ICommandHandler<CancelOrderCommand, CancelOrderResponseDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMatchingEngineQueue _engineQueue;

    public CancelOrderCommandHandler(
        IOrderRepository orderRepository,
        IMatchingEngineQueue engineQueue)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _engineQueue = engineQueue ?? throw new ArgumentNullException(nameof(engineQueue));
    }

    public async Task<CancelOrderResponseDto> Handle(
        CancelOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
        {
            return new CancelOrderResponseDto
            {
                OrderId = request.OrderId,
                Success = false,
                Message = "Order not found"
            };
        }

        if (order.UserId != request.UserId)
        {
            return new CancelOrderResponseDto
            {
                OrderId = request.OrderId,
                Success = false,
                Message = "Order does not belong to user"
            };
        }

        // Update domain state
        order.Cancel();
        await _orderRepository.UpdateAsync(order, cancellationToken);

        // Notify engine to remove from book
        var cancelCommand = new TradingEngine.MatchingEngine.Commands.CancelOrderCommand
        {
            OrderId = order.Id,
            Symbol = new Symbol(order.Symbol.Value)
        };

        await _engineQueue.EnqueueAsync(cancelCommand, cancellationToken);

        return new CancelOrderResponseDto
        {
            OrderId = order.Id,
            Success = true,
            Message = "Cancel request accepted"
        };
    }
}
