using TradingEngine.MatchingEngine.Commands;
using TradingEngine.MatchingEngine.Models;

namespace TradingEngine.MatchingEngine.Services;

public sealed class MatchingEngineProcessor
{
    private readonly Dictionary<string, (SymbolEngine Engine, object Sync)> _engines = new();
    private readonly Lock _globalSync = new();

    private long _sequenceId;

    public ExecutionResult Process(MatchingEngineCommand command, long engineTimestamp)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(command.Symbol);

        var sequenceId = Interlocked.Increment(ref _sequenceId);
        var symbolKey = command.Symbol.Value;

        var (engine, sync) = GetOrCreateEngine(symbolKey);

        lock (sync)
        {
            return engine.Process(command, sequenceId, engineTimestamp);
        }
    }

    public IReadOnlyList<string> GetActiveSymbols()
    {
        lock (_globalSync)
        {
            return _engines.Keys.ToArray();
        }
    }

    private (SymbolEngine Engine, object Sync) GetOrCreateEngine(string symbol)
    {
        lock (_globalSync)
        {
            if (!_engines.TryGetValue(symbol, out var entry))
            {
                entry = (new SymbolEngine(symbol), new object());
                _engines[symbol] = entry;
            }

            return entry;
        }
    }
}
