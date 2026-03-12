namespace TradingPlatform.Application.Common;

/// <summary>
/// Base interface for MediatR queries.
/// </summary>
/// <typeparam name="TResponse">The response type.</typeparam>
public interface IQuery<out TResponse>
{
}
