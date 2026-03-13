using MediatR;
using Microsoft.AspNetCore.Mvc;
using TradingEngineApi.Common;
using TradingEngineApi.Extensions;
using TradingPlatform.Application.Accounts.Commands;
using TradingPlatform.Application.Accounts.Dtos;

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
}