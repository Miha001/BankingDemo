namespace BankingDemo.Domain.Exceptions;

public class ClientNotExist(Guid clientId) : BaseException($"Пользователя с id={clientId} не существует");