using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using TradingEngine.MatchingEngine.Abstractions;
using TradingEngine.MatchingEngine.Commands;
using TradingEngine.MatchingEngine.Services;
using TradingEngine.MatchingEngine.Services.Background;

namespace TradingEngine.MatchingEngine;

public static class MatchingEngineDependencyInjection
{
    private const int ChannelCapacity = 1000;

    public static MatchingEngineBuilder AddMatchingEngine(this IServiceCollection services)
    {
        var channel = Channel.CreateBounded<MatchingEngineCommand>(new BoundedChannelOptions(ChannelCapacity)
        {
            SingleReader = true,
            SingleWriter = false,
            FullMode = BoundedChannelFullMode.Wait
        });

        services.AddSingleton(channel.Reader);
        services.AddSingleton<IMatchingEngineQueue>(_ => new MatchingEngineQueue(channel.Writer));
        services.AddSingleton<MatchingEngineProcessor>();
        services.AddSingleton<IEngineTimeProvider, StopwatchEngineTimeProvider>();
        services.AddSingleton<IExecutionResultDispatcher, ExecutionResultDispatcher>();
        services.AddSingleton<MatchingEngineWorker>();
        services.AddHostedService<MatchingEngineBackgroundService>();

        return new MatchingEngineBuilder(services);
    }

    public static MatchingEngineBuilder AddHandler<THandler>(this MatchingEngineBuilder builder)
        where THandler : class, IExecutionResultHandler
    {
        builder.Services.AddScoped<IExecutionResultHandler, THandler>();
        return builder;
    }
}
