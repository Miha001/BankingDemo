using BankingDemo.Application.Features.Commands;
using BankingDemo.Application.Features.Queries;
using BankingDemo.Domain.Models.Requests;
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
        [FromBody] CreditRequest req,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new CreditCommand(req.Id, req.ClientId, req.DateTime, req.Amount);
        var result = await mediator.Send(command, ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> Debit(
        [FromBody] DebitRequest req,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new DebitCommand(req.Id, req.ClientId, req.DateTime, req.Amount);
        var result = await mediator.Send(command, ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> Revert(
        [FromQuery] Guid id,
        IMediator mediator,
        CancellationToken ct)
    {
        var result = await mediator.Send(new RevertCommand(id), ct);

        return Results.Ok(new
        {
            revertDateTime = result.InsertDateTime,
            clientBalance = result.ClientBalance
        });
    }

    private static async Task<IResult> GetBalance(
        [FromQuery] Guid id,
        IMediator mediator,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetBalanceQuery(id), ct);

        return Results.Ok(new
        {
            balanceDateTime = result.InsertDateTime,
            clientBalance = result.ClientBalance
        });
    }
}