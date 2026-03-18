using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TradingEngine.Application.Accounts.Dtos;
using TradingPlatform.Application.Common;
using TradingEngine.Application.Interfaces.Accounts;

namespace TradingEngine.Application.Accounts.Queries
{
    public class GetAccountByIdQuery : IQuery<Result<AccountViewDto?>> , IRequest<Result<AccountViewDto?>>
    {
        public Guid AccountId { get; set; }
    }

    public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, Result<AccountViewDto?>>
    {
        private readonly IAccountReadRepository _readRepository;

        public GetAccountByIdQueryHandler(IAccountReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task<Result<AccountViewDto?>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var dto = await _readRepository.GetByIdAsync(request.AccountId, cancellationToken);

            if (dto == null)
                return Result<AccountViewDto?>.Failure("Account not found");

            return Result<AccountViewDto?>.Success(dto);
        }
    }
}
