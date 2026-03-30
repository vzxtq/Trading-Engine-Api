using System.Threading.Channels;
using TradingEngine.MatchingEngine.Abstractions;
using TradingEngine.MatchingEngine.Commands;

namespace TradingEngine.MatchingEngine.Services;

public sealed class MatchingEngineQueue : IMatchingEngineQueue
{
    private readonly ChannelWriter<MatchingEngineCommand> _writer;

    public MatchingEngineQueue(ChannelWriter<MatchingEngineCommand> writer)
    {
        _writer = writer ?? throw new ArgumentNullException(nameof(writer));
    }

    /// <summary>
    /// Enqueues a command for processing.
    /// Blocks if the queue is full (backpressure).
    /// </summary>
    public ValueTask EnqueueAsync(MatchingEngineCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        return _writer.WriteAsync(command, cancellationToken);
    }
}
