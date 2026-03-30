using System.Diagnostics;
using TradingEngine.MatchingEngine.Abstractions;
using TradingEngine.MatchingEngine.Models;

namespace TradingEngine.MatchingEngine.Handlers;

public class PersistenceExecutionResultHandler : IExecutionResultHandler
{
    public async Task HandleAsync(ExecutionResult result, CancellationToken cancellationToken)
    {
        Task handlerTask = result switch
        {
            ExecutionResult.Accepted accepted => HandleAcceptedAsync(accepted, cancellationToken),
            ExecutionResult.Rejected => Task.CompletedTask,
            _ => throw new UnreachableException()
        };

        await handlerTask;
    }

    private Task HandleAcceptedAsync(ExecutionResult.Accepted accepted, CancellationToken cancellationToken)
    {
        // TODO: inject DbContext/repositories and persist trades & order states atomically.
        return Task.CompletedTask;
    }
}
