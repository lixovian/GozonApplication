using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrdersService.Infrastructure.Data;
using OrdersService.Infrastructure.ExternalServices.PaymentsService;

namespace OrdersService.Infrastructure.Jobs;

internal sealed class OutboxPublisherBackgroundService(
    IServiceScopeFactory scopeFactory,
    IProducer<Null, PaymentRequestedDto> producer,
    IOptions<OrdersProducerOptions> options,
    ILogger<OutboxPublisherBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);

                using var scope = scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();

                var batch = await dbContext.Outbox
                    .Where(x => x.SentAt == null && x.Type == "PaymentRequested")
                    .OrderBy(x => x.CreatedAt)
                    .Take(20)
                    .ToListAsync(stoppingToken);

                foreach (var msg in batch)
                {
                    var dto = System.Text.Json.JsonSerializer.Deserialize<UseCases.Orders.AddOrder.PaymentRequestedOutboxMessage>(msg.Payload);
                    if (dto is null)
                        continue;

                    var brokerDto = new PaymentRequestedDto(dto.OrderId, dto.UserId, dto.Amount, dto.Description, dto.Key);

                    await producer.ProduceAsync(
                        options.Value.Topic,
                        new Message<Null, PaymentRequestedDto> { Value = brokerDto },
                        stoppingToken);

                    msg.SentAt = DateTimeOffset.UtcNow;
                }

                if (batch.Count > 0)
                    await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Outbox publisher loop error");
            }
        }
    }
}
