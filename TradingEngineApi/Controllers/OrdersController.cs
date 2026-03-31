using MediatR;
using Microsoft.AspNetCore.Mvc;
using TradingEngine.Application.Features.Orders.Commands;
using TradingEngine.Application.Features.Orders.Queries;
using TradingEngineApi.Extensions;

namespace TradingEngineApi.Controllers;

[Route("api/[controller]")]
public class OrdersController : ApiController
{
    public OrdersController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(
        [FromBody] PlaceOrderCommand command,
        CancellationToken ct)
    {
        var result = await Mediator.Send(command, ct);
        return result.ToActionResult();
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> CancelOrder(
        Guid id,
        [FromBody] CancelOrderCommand command,
        CancellationToken ct)
    {
        command.OrderId = id;
        var result = await Mediator.Send(command, ct);
        return result.ToActionResult();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var query = new GetOrderByIdQuery { OrderId = id };
        var result = await Mediator.Send(query, ct);

        return result.ToActionResult();
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUser(Guid userId, CancellationToken ct)
    {
        var query = new GetUserOrdersQuery { UserId = userId };
        var result = await Mediator.Send(query, ct);

        return result.ToActionResult();
    }

    [HttpGet("book/{symbol}")]
    public async Task<IActionResult> GetOrderBook(string symbol, CancellationToken ct)
    {
        var query = new GetOrderBookQuery { Symbol = symbol };
        var result = await Mediator.Send(query, ct);

        return result.ToActionResult();
    }
}
