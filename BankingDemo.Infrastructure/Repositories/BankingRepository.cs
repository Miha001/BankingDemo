using BankingDemo.Application.Abstractions;
using BankingDemo.Application.Features.Dto;
using BankingDemo.Domain.Entities;
using BankingDemo.Domain.Exceptions;
using BankingDemo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BankingDemo.Infrastructure.Repositories;

public class BankingRepository(FinanceDbContext db) : IBankingRepository
{
    private async Task<TransactionResultDto> ProcessLock(Guid clientId, Func<Client, Task<TransactionResultDto>> action, CancellationToken ct)
    {
        var strategy = db.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            using var transaction = await db.Database.BeginTransactionAsync(ct);
            try
            {
                var client = await db.Clients
                    .FromSqlRaw("SELECT \"Id\", \"Balance\" FROM \"Clients\" WHERE \"Id\" = {0} FOR UPDATE", clientId)
                    .FirstOrDefaultAsync(ct);

                //Создаем пользователя, если отсутствуют
                if (client == null)
                {
                    client = new Client(clientId);
                    db.Clients.Add(client);
                }

                var result = await action(client);
                await db.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
                return result;
            }
            catch(InternalException ex)
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        });
    }

    public async Task<TransactionResultDto> ProcessCreditAsync(Guid id, Guid clientId, decimal amount, DateTime cDate, DateTime sDate, CancellationToken ct)
    {
        if (await db.Transactions.AnyAsync(t => t.Id == id, ct))
            return await GetSnapshotAsync(id, clientId, ct);

        return await ProcessLock(clientId, async (client) =>
        {
            // Повторная проверка внутри лока
            if (await db.Transactions.AnyAsync(t => t.Id == id, ct))
                return await GetSnapshotAsync(id, clientId, ct);

            client.Credit(amount);
            db.Transactions.Add(new CreditTransaction(id, clientId, amount, cDate, sDate));
            return new TransactionResultDto(sDate, client.Balance);
        }, ct);
    }

    public async Task<TransactionResultDto> ProcessDebitAsync(Guid id, Guid clientId, decimal amount, DateTime cDate, DateTime sDate, CancellationToken ct)
    {
        if (await db.Transactions.AnyAsync(t => t.Id == id, ct))
            return await GetSnapshotAsync(id, clientId, ct);

        return await ProcessLock(clientId, async (client) =>
        {
            if (await db.Transactions.AnyAsync(t => t.Id == id, ct))
                return await GetSnapshotAsync(id, clientId, ct);

            client.Debit(amount);
            db.Transactions.Add(new DebitTransaction(id, clientId, amount, cDate, sDate));
            return new TransactionResultDto(sDate, client.Balance);
        }, ct);
    }

    public async Task<TransactionResultDto> RevertAsync(Guid transactionId, DateTime serverDate, CancellationToken ct)
    {
        var tx = await db.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId, ct)
            ?? throw new TransactionNotExistException();

        if (tx.IsReverted)
            return await GetClientBalanceAsync(tx.ClientId, ct);

        return await ProcessLock(tx.ClientId, async (client) =>
        {
            tx.IsReverted = true;

            // Отмена кредита = списание, отмена дебита = зачисление
            if (tx is CreditTransaction)
            {
                client.Debit(tx.Amount);
            }
            else
            {
                client.Credit(tx.Amount);
            }

            return new TransactionResultDto(serverDate, client.Balance);
        }, ct);
    }

    public async Task<TransactionResultDto> GetClientBalanceAsync(Guid clientId, CancellationToken ct)
    {
        var client = await db.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.Id == clientId, ct);

        if(client is null)
        {
            throw new ClientNotExistException(clientId);
        }

        return new TransactionResultDto(DateTime.UtcNow, client?.Balance ?? 0);
    }

    /// <summary>
    /// Реализует идемпотентность при повторном запросе транзакции
    /// </summary>
    private async Task<TransactionResultDto> GetSnapshotAsync(Guid txId, Guid clientId, CancellationToken ct)
    {
        var transaction = await db.Transactions.AsNoTracking().FirstAsync(t => t.Id == txId, ct);
        var client = await db.Clients.AsNoTracking().FirstAsync(c => c.Id == clientId, ct);
        return new TransactionResultDto(transaction.InsertDateTime, client.Balance);
    }
}