using TradingPlatform.Domain.Common;
using TradingPlatform.Domain.ValueObjects;

namespace TradingPlatform.Infrastructure.Persistence
{
    public class UserAccount : BaseEntity
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public Money Balance { get; set; } = Money.Zero();
        public Money ReservedBalance { get; set; } = Money.Zero();
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; }
    }
}
