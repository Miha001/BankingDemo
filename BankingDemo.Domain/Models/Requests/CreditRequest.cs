namespace BankingDemo.Domain.Models.Requests;

public record CreditRequest(
    Guid Id,
    Guid ClientId,
    DateTime DateTime,
    decimal Amount
);