using MediatR;
using Microsoft.AspNetCore.Mvc;
using TradingEngineApi.Common;
using TradingEngineApi.Extensions;
using TradingEngine.Application.Features.Accounts.Commands;
using TradingEngine.Application.Features.Accounts.Queries;

namespace TradingEngineApi.Controllers;

/// <summary>
/// Controller responsible for managing user accounts.
/// Uses MediatR to dispatch CQRS commands to the application layer.
/// </summary>
[Route("api/[controller]")]
public class AccountsController : ApiController
{
    public AccountsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount(
        CreateAccountCommand command,
        CancellationToken ct)
    {
        var result = await Mediator.Send(command, ct);
        return result.ToActionResult();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var query = new GetAccountByIdQuery { AccountId = id };
        var result = await Mediator.Send(query, ct);
        return result.ToActionResult();
    }
}