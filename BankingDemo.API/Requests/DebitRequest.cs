namespace BankingDemo.API.Requests;

public record DebitRequest(
    Guid Id,
    Guid ClientId,
    DateTime DateTime,
    decimal Amount
);