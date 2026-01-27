using BankingDemo.Application.Abstractions;
using BankingDemo.Application.Features.Dto;
using MediatR;

namespace BankingDemo.Application.Features.Commands;

public record DebitCommand(Guid Id, Guid ClientId, DateTime DateTime, decimal Amount) : IRequest<TransactionResultDto>;

public class DebitCommandHandler(
    IBankingRepository repository,
    TimeProvider timeProvider) : IRequestHandler<DebitCommand, TransactionResultDto>
{
    public async Task<TransactionResultDto> Handle(DebitCommand request, CancellationToken ct)
    {
        return await repository.ProcessDebitAsync(
            request.Id, request.ClientId, request.Amount, request.DateTime, timeProvider.GetUtcNow().UtcDateTime, ct);
    }
}