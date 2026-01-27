using BankingDemo.Application.Abstractions;
using BankingDemo.Application.Features.Dto;
using MediatR;

namespace BankingDemo.Application.Features.Commands;

public record RevertCommand(Guid Id) : IRequest<TransactionResultDto>;

public class RevertCommandHandler(IBankingRepository repository, TimeProvider timeProvider)
    : IRequestHandler<RevertCommand, TransactionResultDto>
{
    public async Task<TransactionResultDto> Handle(RevertCommand request, CancellationToken ct)
    {
        return await repository.RevertAsync(request.Id, timeProvider.GetUtcNow().UtcDateTime, ct);
    }
}