namespace TradingEngine.MatchingEngine.Models;

public class PriceLevel
{
    private readonly Queue<EngineOrder> _orders = new();

    public long Price { get; }
    public IReadOnlyList<EngineOrder> Orders => _orders.ToList().AsReadOnly();
    public long TotalQuantity => _orders.Sum(o => o.RemainingQuantity);

    public PriceLevel(long price)
    {
        if (price <= 0) throw new ArgumentOutOfRangeException(nameof(price));
        Price = price;
    }

    public void AddOrder(EngineOrder order)
    {
        _orders.Enqueue(order);
    }

    public bool RemoveOrder(Guid orderId)
    {
        var count = _orders.Count;
        var temp = new List<EngineOrder>();

        while (_orders.Count > 0)
        {
            var order = _orders.Dequeue();
            if (order.Id != orderId)
                temp.Add(order);
        }

        foreach (var order in temp)
            _orders.Enqueue(order);

        return _orders.Count < count;
    }

    public bool HasOrders => _orders.Count > 0;

    public EngineOrder? PeekOrder() => _orders.Count > 0 ? _orders.Peek() : null;
}
