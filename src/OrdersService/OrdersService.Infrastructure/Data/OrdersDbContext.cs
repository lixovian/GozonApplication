using Microsoft.EntityFrameworkCore;
using OrdersService.Infrastructure.Data.Dtos;

namespace OrdersService.Infrastructure.Data;

internal sealed class OrdersDbContext(DbContextOptions<OrdersDbContext> options) : DbContext(options)
{
    public DbSet<OrderDto> Orders { get; set; }
    public DbSet<OutboxMessageDto> Outbox { get; set; }
    public DbSet<InboxMessageDto> Inbox { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderDto>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("Orders");

            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Amount).IsRequired();

            builder.Property(x => x.Description)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();
        });

        modelBuilder.Entity<OutboxMessageDto>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("OrdersOutbox");

            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Key).IsRequired();
            builder.Property(x => x.Payload).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();

            builder.Property(x => x.SentAt);

            builder.HasIndex(x => x.Key).IsUnique();
            builder.HasIndex(x => x.SentAt);
        });
        
        modelBuilder.Entity<InboxMessageDto>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("OrdersInbox");

            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Key).IsRequired();
            builder.Property(x => x.Payload).IsRequired();
            builder.Property(x => x.ReceivedAt).IsRequired();

            builder.HasIndex(x => x.Key).IsUnique();
        });
    }
}