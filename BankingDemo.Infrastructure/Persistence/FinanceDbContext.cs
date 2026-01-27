using BankingDemo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BankingDemo.Infrastructure.Persistence;

public class FinanceDbContext(DbContextOptions<FinanceDbContext> options, IConfiguration configuration) : DbContext(options)
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("Default"), b => b.EnableRetryOnFailure()
        .MigrationsAssembly(typeof(FinanceDbContext).Assembly.FullName));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>()
            .HasDiscriminator<string>("Type")
            .HasValue<CreditTransaction>("Credit")
            .HasValue<DebitTransaction>("Debit");

        modelBuilder.Entity<Client>().HasIndex(c => c.Id);
    }
}