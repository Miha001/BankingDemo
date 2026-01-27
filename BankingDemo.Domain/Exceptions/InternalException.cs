using System.Net;

namespace BankingDemo.Domain.Exceptions;

public abstract class InternalException(string message)
    : Exception(message)
{
    /// <summary>
    /// Код состояния ответа HTTP
    /// </summary>
    public abstract HttpStatusCode HttpStatusCode { get; }
}