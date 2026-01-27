using BankingDemo.Application.Abstractions;
using BankingDemo.Application.Features.Dto;
using BankingDemo.Domain.Exceptions;
using MediatR;

namespace BankingDemo.Application.Features.Commands;


public record CreditCommand(Guid Id, Guid ClientId, DateTime DateTime, decimal Amount) : IRequest<TransactionResultDto>;

public class CreditCommandHandler(
    IBankingRepository repository,
    TimeProvider timeProvider) : IRequestHandler<CreditCommand, TransactionResultDto>
{
    public async Task<TransactionResultDto> Handle(CreditCommand request, CancellationToken ct)
    {
        return await repository.ProcessCreditAsync(
            request.Id, request.ClientId, request.Amount, request.DateTime, timeProvider.GetUtcNow().UtcDateTime, ct);
    }
}