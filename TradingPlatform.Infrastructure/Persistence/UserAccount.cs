using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPlatform.Domain.ValueObjects;

namespace TradingPlatform.Infrastructure.Persistence
{
    public class UserAccount
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public Money Balance { get; set; } = Money.Zero();
        public Money ReservedBalance { get; set; } = Money.Zero();
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
