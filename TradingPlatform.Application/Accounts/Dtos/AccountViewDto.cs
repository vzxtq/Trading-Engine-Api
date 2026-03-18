using TradingPlatform.Domain.ValueObjects;

namespace TradingEngine.Application.Accounts.Dtos
{
    public class AccountViewDto
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required Money Balance { get; set; }
        public Money? ReservedBalance { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; }
    }
}
