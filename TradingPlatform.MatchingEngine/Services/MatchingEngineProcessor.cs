using TradingEngine.MatchingEngine.Commands;
using TradingEngine.MatchingEngine.Models;
using TradingPlatform.Domain.ValueObjects;

namespace TradingEngine.MatchingEngine.Services;

public sealed class MatchingEngineProcessor
{
    private readonly Dictionary<string, SymbolEngine> _engines = new();

    private static long _sequenceId;

    public ExecutionResult Process(MatchingEngineCommand command, long engineTimestamp)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(command.Symbol);

        var sequenceId = Interlocked.Increment(ref _sequenceId);
        var (engine, _) = GetOrCreateEngine(command.Symbol);
        return engine.Process(command, sequenceId, engineTimestamp);
    }

    public OrderBookSnapshot GetSnapshot(Symbol symbol)
    {
        ArgumentNullException.ThrowIfNull(symbol);

        var symbolKey = symbol.Value;
        if (!_engines.TryGetValue(symbolKey, out var engine))
            return new OrderBookSnapshot(symbolKey, [], []);

        return engine.Snapshot();
    }

    private (SymbolEngine Engine, bool IsNew) GetOrCreateEngine(Symbol symbol)
    {
        var symbolKey = symbol.Value;

        if (_engines.TryGetValue(symbolKey, out var existing))
            return (existing, false);

        var created = new SymbolEngine(symbolKey);
        _engines[symbolKey] = created;
        return (created, true);
    }
}
