using TradingPlatform.Application.Common;

namespace TradingPlatform.Application.Accounts.Commands;

/// <summary>
/// Command to create a new user account.
/// </summary>
public class CreateAccountCommand : ICommand<CreateAccountResponse>
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal InitialBalance { get; set; }
}

public class CreateAccountResponse
{
    public Guid AccountId { get; set; }
    public string Email { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}