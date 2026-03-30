using TradingPlatform.Domain.Enums;
using TradingPlatform.Domain.ValueObjects;

namespace TradingEngine.MatchingEngine.Commands;

/// <summary>
/// Immutable command envelope consumed by the matching engine.
/// </summary>
public abstract record MatchingEngineCommand
{
    public required Symbol Symbol { get; init; }
}

public sealed record AddOrderCommand : MatchingEngineCommand
{
    public required Guid OrderId { get; init; }
    public required Guid UserId { get; init; }
    public required long Price { get; init; }
    public required long Quantity { get; init; }
    public required OrderSide Side { get; init; }
    public required long ReceivedAt { get; init; }
}

public sealed record CancelOrderCommand : MatchingEngineCommand
{
    public required Guid OrderId { get; init; }
}
