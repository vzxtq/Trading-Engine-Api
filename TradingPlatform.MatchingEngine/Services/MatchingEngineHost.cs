using System.Linq;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TradingEngine.MatchingEngine.Abstractions;
using TradingEngine.MatchingEngine.Commands;
using TradingEngine.MatchingEngine.Models;
using TradingPlatform.Domain.ValueObjects;

namespace TradingEngine.MatchingEngine.Services;

internal sealed record MatchingEngineShard(
    int Id,
    Channel<MatchingEngineCommand> Channel,
    MatchingEngineWorker Worker)
{
    public ChannelWriter<MatchingEngineCommand> Writer => Channel.Writer;
}

/// <summary>
/// Orchestrates sharded, per-symbol workers. Symbols are deterministically mapped to shards by hash.
/// </summary>
public sealed class MatchingEngineHost : IMatchingEngineQueue, IOrderBookSnapshotProvider
{
    private readonly MatchingEngineShard[] _shards;
    private readonly MatchingEngineOptions _options;

    public MatchingEngineHost(
        IOptions<MatchingEngineOptions> options,
        IExecutionResultDispatcher dispatcher,
        IEngineTimeProvider timeProvider,
        ILoggerFactory loggerFactory)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        var shardCount = Math.Max(1, _options.ShardCount);

        _shards = new MatchingEngineShard[shardCount];

        for (var i = 0; i < shardCount; i++)
        {
            var channel = Channel.CreateBounded<MatchingEngineCommand>(new BoundedChannelOptions(_options.ChannelCapacity)
            {
                SingleReader = true,
                SingleWriter = false,
                FullMode = _options.FullMode
            });

            var processor = new MatchingEngineProcessor();
            var workerLogger = loggerFactory.CreateLogger<MatchingEngineWorker>();
            var worker = new MatchingEngineWorker(processor, dispatcher, timeProvider, channel.Reader, workerLogger);

            _shards[i] = new MatchingEngineShard(i, channel, worker);
        }
    }

    public ValueTask EnqueueAsync(MatchingEngineCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        return GetWriter(command.Symbol.Value).WriteAsync(command, cancellationToken);
    }

    public async Task<OrderBookSnapshot> GetSnapshotAsync(Symbol symbol, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(symbol);

        var tcs = new TaskCompletionSource<OrderBookSnapshot>(TaskCreationOptions.RunContinuationsAsynchronously);
        var cmd = new SnapshotOrderBookCommand { Symbol = symbol, Completion = tcs };

        await GetWriter(symbol.Value).WriteAsync(cmd, cancellationToken).ConfigureAwait(false);
        return await tcs.Task.WaitAsync(cancellationToken).ConfigureAwait(false);
    }

    public Task RunAsync(CancellationToken ct)
    {
        var tasks = _shards.Select(shard => shard.Worker.RunAsync(ct)).ToArray();
        return Task.WhenAll(tasks);
    }

    private ChannelWriter<MatchingEngineCommand> GetWriter(string symbol)
    {
        var index = GetShardIndex(symbol);
        return _shards[index].Writer;
    }

    private int GetShardIndex(string symbol)
    {
        var hash = StringComparer.OrdinalIgnoreCase.GetHashCode(symbol);
        return Math.Abs(hash % _shards.Length);
    }
}
