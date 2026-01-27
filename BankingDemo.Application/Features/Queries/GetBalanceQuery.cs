using BankingDemo.Application.Abstractions;
using BankingDemo.Application.Features.Dto;
using MediatR;

namespace BankingDemo.Application.Features.Queries;

public record GetBalanceQuery(Guid ClientId) : IRequest<TransactionResultDto>;

public class GetBalanceQueryHandler(IBankingRepository repository) : IRequestHandler<GetBalanceQuery, TransactionResultDto>
{
    public async Task<TransactionResultDto> Handle(GetBalanceQuery query, CancellationToken ct)
    {
        return await repository.GetClientBalanceAsync(query.ClientId, ct);
    }
}