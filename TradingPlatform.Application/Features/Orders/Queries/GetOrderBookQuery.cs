using TradingEngine.Application.Common;
using TradingEngine.Application.Features.Orders.Repositories;
using TradingPlatform.Application.Features.Orders.Dtos;
using TradingPlatform.Domain.ValueObjects;

namespace TradingEngine.Application.Features.Orders.Queries;

public class GetOrderBookQuery : IQuery<OrderBookDto>
{
    public string Symbol { get; set; } = string.Empty;
}

public sealed class GetOrderBookQueryHandler : IQueryHandler<GetOrderBookQuery, OrderBookDto>
{
    private readonly IOrderBookReadRepository _readRepository;

    public GetOrderBookQueryHandler(IOrderBookReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<OrderBookDto> Handle(GetOrderBookQuery request, CancellationToken cancellationToken)
    {
        var symbol = new Symbol(request.Symbol);
        return await _readRepository.GetOrderBookAsync(symbol, cancellationToken);
    }
}
