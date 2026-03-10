namespace TradingPlatform.Domain.Common;

/// <summary>
/// Base class for all domain entities following DDD principles.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Unique identifier for the entity.
    /// </summary>
    public Guid Id { get; protected set; } = Guid.NewGuid();

    /// <summary>
    /// Timestamp when the entity was created.
    /// </summary>
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Timestamp when the entity was last modified.
  /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
