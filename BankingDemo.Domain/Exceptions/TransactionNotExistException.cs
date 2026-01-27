using System.Net;

namespace BankingDemo.Domain.Exceptions;

public class TransactionNotExistException() : InternalException("Транзакция не найдена")
{
    /// <inheritdoc/>
    public override HttpStatusCode HttpStatusCode => HttpStatusCode.UnprocessableContent;
}