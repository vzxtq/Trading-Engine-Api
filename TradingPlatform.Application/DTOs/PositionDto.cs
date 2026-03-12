namespace TradingPlatform.Application.DTOs;

/// <summary>
/// DTO for transferring position information.
/// </summary>
public class PositionDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal AverageCost { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}