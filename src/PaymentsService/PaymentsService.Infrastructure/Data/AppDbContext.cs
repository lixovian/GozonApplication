using Microsoft.EntityFrameworkCore;
using PaymentsService.Entities.Models;
using PaymentsService.Infrastructure.Outbox.Dtos;

namespace PaymentsService.Infrastructure.Data;

internal sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AccountDto> Accounts { get; set; }
    public DbSet<AccountTransactionDto> AccountTransactions { get; set; }

    public DbSet<InboxMessage> Inbox { get; set; }
    public DbSet<OutboxMessage> Outbox { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountDto>(builder =>
        {
            builder.ToTable("accounts");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
            builder.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
            builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

            builder.HasIndex(x => x.UserId).IsUnique();
        });

        modelBuilder.Entity<AccountTransactionDto>(builder =>
        {
            builder.ToTable("account_transactions");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
            builder.Property(x => x.AccountId).HasColumnName("account_id").IsRequired();
            builder.Property(x => x.Type).HasColumnName("type").IsRequired();
            builder.Property(x => x.Amount).HasColumnName("amount").IsRequired();
            builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
            builder.Property(x => x.Key).HasColumnName("key").IsRequired();

            builder.HasIndex(x => x.Key).IsUnique(); // idempotency for any money movement
            builder.HasIndex(x => x.AccountId);
        });

        modelBuilder.Entity<InboxMessage>(builder =>
        {
            builder.ToTable("payments_inbox");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
            builder.Property(x => x.Type).HasColumnName("type").IsRequired();      // PaymentRequested
            builder.Property(x => x.Key).HasColumnName("key").IsRequired();
            builder.Property(x => x.Payload).HasColumnName("payload").IsRequired();
            builder.Property(x => x.ReceivedAt).HasColumnName("received_at").IsRequired();

            builder.HasIndex(x => x.Key).IsUnique();
        });

        modelBuilder.Entity<OutboxMessage>(builder =>
        {
            builder.ToTable("payments_outbox");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
            builder.Property(x => x.Type).HasColumnName("type").IsRequired();      // PaymentSucceeded/PaymentFailed
            builder.Property(x => x.Key).HasColumnName("key").IsRequired();
            builder.Property(x => x.Payload).HasColumnName("payload").IsRequired();
            builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
            builder.Property(x => x.SentAt).HasColumnName("sent_at");

            builder.HasIndex(x => x.Key);
            builder.HasIndex(x => x.SentAt);
        });
    }
}

internal sealed record AccountDto(Guid Id)
{
    public int UserId { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}

internal sealed record AccountTransactionDto(Guid Id)
{
    public Guid AccountId { get; init; }
    public string Type { get; init; } = "TOPUP";
    public decimal Amount { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string Key { get; init; } = string.Empty;
}
