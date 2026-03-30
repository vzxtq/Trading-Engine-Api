using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngine.Application.Features.Accounts.Dtos
{
    public class CreateAccountResponseDto
    {
        public Guid AccountId { get; set; }
        public string Email { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }
}
