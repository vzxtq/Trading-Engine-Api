namespace TradingPlatform.Application.DTOs;

/// <summary>
/// DTO for transferring user account information.
/// </summary>
public class UserAccountDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}