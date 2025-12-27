using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaymentsService.Infrastructure.Data;
using PaymentsService.Infrastructure.Kafka.Dtos;
using PaymentsService.Infrastructure.Outbox.Dtos;
using PaymentsService.UseCases.Payments.ProcessPayment;

namespace PaymentsService.Infrastructure.Kafka;

internal sealed class PaymentsOutboxPublisher(
    IServiceScopeFactory scopeFactory,
    IProducer<Null, PaymentResultDto> producer,
    IOptions<PaymentsProducerOptions> options,
    ILogger<PaymentsOutboxPublisher> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);

                using var scope = scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var batch = await dbContext.Outbox
                    .Where(x => x.SentAt == null)
                    .OrderBy(x => x.CreatedAt)
                    .Take(20)
                    .ToListAsync(stoppingToken);

                foreach (var msg in batch)
                {
                    var response = System.Text.Json.JsonSerializer.Deserialize<ProcessPaymentResponse>(msg.Payload);
                    if (response is null)
                        continue;

                    var dto = new PaymentResultDto
                    {
                        OrderId = response.OrderId,
                        Key = response.Key,
                        Result = response.Result,
                        Reason = response.Reason
                    };

                    await producer.ProduceAsync(
                        options.Value.Topic,
                        new Message<Null, PaymentResultDto> { Value = dto },
                        stoppingToken);

                    msg.SentAt = DateTimeOffset.UtcNow;
                }

                if (batch.Count > 0)
                    await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Payments outbox publisher loop error");
            }
        }
    }
}
