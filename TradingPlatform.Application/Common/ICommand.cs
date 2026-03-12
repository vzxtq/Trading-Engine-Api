namespace TradingPlatform.Application.Common;

/// <summary>
/// Base interface for MediatR commands.
/// </summary>
public interface ICommand
{
}

/// <summary>
/// Base interface for MediatR commands with a result.
/// </summary>
/// <typeparam name="TResponse">The response type.</typeparam>
public interface ICommand<out TResponse> : ICommand
{
}