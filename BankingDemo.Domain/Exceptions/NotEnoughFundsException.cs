using System.Net;

namespace BankingDemo.Domain.Exceptions;

public class NotEnoughFundsException() : InternalException("У клиента нельзя списать больше средств, чем есть на балансе")
{
    /// <inheritdoc/>
    public override HttpStatusCode HttpStatusCode { get; } = HttpStatusCode.UnprocessableContent;
}