using System.Diagnostics;
using TradingEngine.MatchingEngine.Abstractions;
using TradingEngine.MatchingEngine.Models;

namespace TradingEngine.MatchingEngine.Handlers;

public class MarketDataExecutionResultHandler : IExecutionResultHandler
{
    public async Task HandleAsync(ExecutionResult result, CancellationToken cancellationToken)
    {
        Task task = result switch
        {
            ExecutionResult.Accepted accepted => HandleAcceptedAsync(accepted, cancellationToken),
            ExecutionResult.Rejected => Task.CompletedTask,
            _ => throw new UnreachableException()
        };

        await task;
    }

    private Task HandleAcceptedAsync(ExecutionResult.Accepted accepted, CancellationToken cancellationToken)
    {
        try
        {
            foreach (var trade in accepted.Trades)
            {
                // publish to realtime / pubsub here
            }

            foreach (var stateChange in accepted.StateChanges)
            {
                // publish order-book updates here
            }
        }
        catch (Exception)
        {
            // Swallow to avoid impacting the engine pipeline; log in real implementation.
        }

        return Task.CompletedTask;
    }
}
