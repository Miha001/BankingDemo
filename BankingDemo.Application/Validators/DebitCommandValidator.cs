using BankingDemo.Application.Features.Commands;
using FluentValidation;

namespace BankingDemo.Application.Validators;

public class DebitCommandValidator : AbstractValidator<DebitCommand>
{
    public DebitCommandValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Transaction ID не может быть пустым");
        RuleFor(x => x.ClientId).NotEmpty().WithMessage("Client ID не может быть пустым");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Сумма должна быть больше нуля");
        RuleFor(x => x.DateTime).LessThanOrEqualTo(timeProvider.GetUtcNow().UtcDateTime).WithMessage("Дата транзакции не может быть указана в будущем");
    }
}