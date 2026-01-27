using BankingDemo.Domain.Abstraction;

namespace BankingDemo.Domain.Entities;

public abstract class Transaction : ITransaction
{
    ///<inheritdoc/>
    public Guid Id { get; private set; }

    ///<inheritdoc/>
    public Guid ClientId { get; private set; }

    ///<inheritdoc/>
    public DateTime DateTime { get; private set; }

    /// <summary>
    /// Дата вставки в БД
    /// </summary>
    public DateTime InsertDateTime { get; private set; }

    ///<inheritdoc/>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Флаг отмены
    /// </summary>
    public bool IsReverted { get; set; }

    protected Transaction(Guid id, Guid clientId, decimal amount, DateTime dateTime, DateTime insertDateTime)
    {
        Id = id;
        ClientId = clientId;
        Amount = amount;
        DateTime = dateTime;
        InsertDateTime = insertDateTime;
    }
}