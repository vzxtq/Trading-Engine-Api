using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using TradingPlatform.Application.Accounts.Commands;
using TradingPlatform.Application.Common;
using TradingPlatform.Application.Interfaces;
using TradingPlatform.Domain.Entities;
using TradingPlatform.Domain.ValueObjects;

namespace TradingPlatform.Application.Accounts.Dtos
{
    public class CreateAccountCommand : ICommand<Result<CreateAccountResponseDto>>
    {
        [Required]
        public required string Email { get; init; }
        [Required]
        public required string Name { get; init; }
        [Required]
        public decimal InitialBalance { get; init; }
        [Required]
        public string Currency { get; init; } = "USD";
    }

    public sealed class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand, Result<CreateAccountResponseDto>>
    {
        private readonly IAccountRepository _accountRepository;

        public CreateAccountCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Result<CreateAccountResponseDto>> Handle( CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var money = new Money(request.InitialBalance, request.Currency);

            var account = UserAccountDomain.Create(
                request.Email,
                request.Name,
                money
            );

            await _accountRepository.AddAsync(account, cancellationToken);

            var response = new CreateAccountResponseDto
            {
                AccountId = account.Id,
                Email = account.Email,
                Balance = account.Balance.Amount
            };

            return Result<CreateAccountResponseDto>.Success(response);
        }
    }
}
