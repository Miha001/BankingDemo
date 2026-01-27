using BankingDemo.Application.Features.Dto;

namespace BankingDemo.Application.Abstractions;

public interface IBankingRepository
{
    Task<TransactionResultDto> ProcessCreditAsync(Guid id, Guid clientId, decimal amount, DateTime clientDate, DateTime serverDate, CancellationToken ct);
    Task<TransactionResultDto> ProcessDebitAsync(Guid id, Guid clientId, decimal amount, DateTime clientDate, DateTime serverDate, CancellationToken ct);
    Task<TransactionResultDto> RevertAsync(Guid transactionId, DateTime serverDate, CancellationToken ct);
    Task<TransactionResultDto> GetClientBalanceAsync(Guid clientId, CancellationToken ct);
}