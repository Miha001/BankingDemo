using BankingDemo.API.Endpoints;
using BankingDemo.API.Middleware;
using BankingDemo.Application.Abstractions;
using BankingDemo.Application.Behaviors;
using BankingDemo.Application.Features.Commands;
using BankingDemo.Infrastructure.Persistence;
using BankingDemo.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting Web API...");

    var builder = WebApplication.CreateBuilder(args);
    
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services));
    
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
    builder.Services.AddDbContext<FinanceDbContext>();
    
    builder.Services.AddScoped<IBankingRepository, BankingRepository>();
    builder.Services.AddSingleton(TimeProvider.System);
    
    builder.Services.AddValidatorsFromAssemblyContaining<CreditCommand>();
    builder.Services.AddValidatorsFromAssemblyContaining<DebitCommand>();
    
    builder.Services.AddMediatR(cfg => {
        cfg.RegisterServicesFromAssembly(typeof(CreditCommand).Assembly);
        cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    });
    
    var app = builder.Build();
    
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Banking Demo API V1");
        c.RoutePrefix = "swagger";
    });
    
    // Auto-migration
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<FinanceDbContext>();
        try { db.Database.Migrate(); } catch { db.Database.EnsureCreated(); }
    }
    
    app.UseSerilogRequestLogging();
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.MapTransactionEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}