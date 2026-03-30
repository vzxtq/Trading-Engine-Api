using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TradingEngine.MatchingEngine.Services.Background;

public sealed class MatchingEngineBackgroundService : BackgroundService
{
    private readonly MatchingEngineWorker _worker;
    private readonly ILogger<MatchingEngineBackgroundService> _logger;

    public MatchingEngineBackgroundService(
        MatchingEngineWorker worker,
        ILogger<MatchingEngineBackgroundService> logger)
    {
        _worker = worker;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Matching Engine starting");
        try
        {
            await _worker.RunAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Matching Engine fatal error");
            throw;
        }
        finally
        {
            _logger.LogInformation("Matching Engine stopped");
        }
    }
}