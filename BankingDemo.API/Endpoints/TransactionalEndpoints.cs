using BankingDemo.Application.Features.Commands;
using BankingDemo.Application.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingDemo.API.Endpoints;

public static class TransactionEndpoints
{
    public static void MapTransactionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(string.Empty)
            .WithTags("Transactions");

        group.MapPost("/credit", Credit);
        group.MapPost("/debit", Debit);

        group.MapPost("/revert", Revert);
        group.MapGet("/balance", GetBalance);
    }

    private static async Task<IResult> Credit(
        [FromBody] CreditCommand command,
        IMediator mediator,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> Debit(
        [FromBody] DebitCommand command,
        IMediator mediator,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> Revert(
        [AsParameters] RevertCommand command,
        IMediator mediator,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);

        return Results.Ok(new
        {
            revertDateTime = result.InsertDateTime,
            clientBalance = result.ClientBalance
        });
    }

    private static async Task<IResult> GetBalance(
        [AsParameters] GetBalanceQuery query,
        IMediator mediator,
        CancellationToken ct)
    {
        var result = await mediator.Send(query, ct);

        return Results.Ok(new
        {
            balanceDateTime = result.InsertDateTime,
            clientBalance = result.ClientBalance
        });
    }
}