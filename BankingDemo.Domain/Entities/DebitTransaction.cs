namespace BankingDemo.Domain.Entities;

public class DebitTransaction(Guid id, Guid clientId, decimal amount, DateTime dateTime, DateTime insertDateTime)
    : Transaction(id, clientId, amount, dateTime, insertDateTime);