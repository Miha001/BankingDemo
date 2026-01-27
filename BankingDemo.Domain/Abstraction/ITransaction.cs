namespace BankingDemo.Domain.Abstraction;

public interface ITransaction
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// 
    /// </summary>
    Guid ClientId { get; }

    /// <summary>
    /// Дата создания транзакции клиентом
    /// </summary>
    DateTime DateTime { get; }

    /// <summary>
    /// 
    /// </summary>
    decimal Amount { get; }
}