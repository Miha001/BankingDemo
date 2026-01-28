namespace BankingDemo.API.Requests;

public record CreditRequest(
    Guid Id,
    Guid ClientId,
    DateTime DateTime,
    decimal Amount
);