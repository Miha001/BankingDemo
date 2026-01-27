namespace BankingDemo.Domain.Entities;

public class CreditTransaction(Guid id, Guid clientId, decimal amount, DateTime dateTime, DateTime insertDateTime)
    : Transaction(id, clientId, amount, dateTime, insertDateTime);