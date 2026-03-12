namespace TradingPlatform.Application.DTOs;

/// <summary>
/// DTO for transferring trade information.
/// </summary>
public class TradeDto
{
    public Guid Id { get; set; }
    public Guid BuyOrderId { get; set; }
    public Guid SellOrderId { get; set; }
    public Guid BuyerId { get; set; }
    public Guid SellerId { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Quantity { get; set; }
    public DateTime ExecutedAt { get; set; }
}