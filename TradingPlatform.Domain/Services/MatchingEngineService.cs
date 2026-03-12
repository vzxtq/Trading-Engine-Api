using TradingPlatform.Domain.Entities;
using TradingPlatform.Domain.Enums;
using TradingPlatform.Domain.ValueObjects;

namespace TradingPlatform.Domain.Services;

/// <summary>
/// Represents the domain service for order matching logic.
/// Core matching engine for the trading platform.
/// Implements price-time priority matching algorithm.
/// </summary>
public class MatchingEngineService
{
    /// <summary>
    /// Attempts to match a new order against existing orders.
    /// Returns a list of matched pairs.
    /// </summary>
    public IReadOnlyList<(Order buyOrder, Order sellOrder, Quantity matchedQuantity)> FindMatches(
        Order incomingOrder,
        IEnumerable<Order> existingOrders)
    {
        var matches = new List<(Order, Order, Quantity)>();

        if (incomingOrder.Symbol is null)
            return matches;

        var relevantOrders = existingOrders
            .Where(o => o.Symbol == incomingOrder.Symbol &&
                        o.Side != incomingOrder.Side &&
                        (o.Status == OrderStatus.Pending || o.Status == OrderStatus.PartiallyFilled) &&
                        incomingOrder.Status != OrderStatus.Cancelled &&
                        incomingOrder.Status != OrderStatus.Rejected)
            .OrderBy(o => o.CreatedAt) // Time priority
            .ToList();

        foreach (var existingOrder in relevantOrders)
        {
            if (incomingOrder.RemainingQuantity.Value == 0)
                break;

            //if (!incomingOrder.CanMatchWith(existingOrder))
            //    continue; //TODO should check via Matching Engine

            var matchedQuantity = DetermineMatchQuantity(incomingOrder, existingOrder);

            matches.Add((
                GetBuyOrder(incomingOrder, existingOrder),
                GetSellOrder(incomingOrder, existingOrder),
                matchedQuantity
            ));
        }

        return matches;
    }

    /// <summary>
    /// Determines the quantity that can be matched between two orders.
    /// </summary>
    private static Quantity DetermineMatchQuantity(Order order1, Order order2)
    {
        var quantity1 = order1.RemainingQuantity.Value;
        var quantity2 = order2.RemainingQuantity.Value;
        var matchedQuantity = Math.Min(quantity1, quantity2);

        return new Quantity(matchedQuantity);
    }

    /// <summary>
    /// Identifies which order is the buy order.
    /// </summary>
    private static Order GetBuyOrder(Order order1, Order order2)
    {
        return order1.Side == OrderSide.Buy ? order1 : order2;
    }

    /// <summary>
    /// Identifies which order is the sell order.
    /// </summary>
    private static Order GetSellOrder(Order order1, Order order2)
    {
        return order1.Side == OrderSide.Sell ? order1 : order2;
    }
}