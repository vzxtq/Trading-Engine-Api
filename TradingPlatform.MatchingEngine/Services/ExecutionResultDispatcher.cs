using TradingEngine.MatchingEngine.Abstractions;
using TradingEngine.MatchingEngine.Models;

namespace TradingEngine.MatchingEngine.Services
{
    public sealed class ExecutionResultDispatcher : IExecutionResultDispatcher
    {
        private readonly IEnumerable<IExecutionResultHandler> _handlers;

        public ExecutionResultDispatcher(IEnumerable<IExecutionResultHandler> handlers)
        {
            _handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
        }

        public async Task DispatchAsync(ExecutionResult result, CancellationToken cancellationToken)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            foreach (var handler in _handlers)
            {
                await handler.HandleAsync(result, cancellationToken);
            }
        }
    }
}