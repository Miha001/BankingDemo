using System.Net;

namespace BankingDemo.Domain.Exceptions;

public class ClientNotExist(Guid clientId) : InternalException($"Пользователя с id={clientId} не существует")
{
    /// <inheritdoc/>
    public override HttpStatusCode HttpStatusCode { get; } = HttpStatusCode.BadRequest;
}