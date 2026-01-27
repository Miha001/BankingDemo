using BankingDemo.Domain.Exceptions;

namespace BankingDemo.Domain.Entities;

public class Client
{
    public Guid Id { get; private set; }
    public decimal Balance { get; private set; }

    public Client(Guid id, decimal balance = 0)
    {
        Id = id;
        Balance = balance;
    }

    public void Credit(decimal amount)
    {
        Balance += amount;
    }

    public void Debit(decimal amount)
    {
        if(amount > Balance)
        {
            throw new NotEnoughFundsException();
        }

        Balance -= amount;
    }
}
