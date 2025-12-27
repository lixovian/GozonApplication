using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaymentsService.Infrastructure.Kafka.Dtos;
using PaymentsService.UseCases.Payments.ProcessPayment;

namespace PaymentsService.Infrastructure.Kafka;

internal sealed class PaymentRequestedConsumer(
    IConsumer<Ignore, PaymentRequestedDto?> consumer,
    ILogger<PaymentRequestedConsumer> logger,
    IServiceScopeFactory serviceScopeFactory,
    IOptions<PaymentsConsumerOptions> options) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        consumer.Subscribe(options.Value.Topic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumed = consumer.Consume(stoppingToken);

                    if (consumed.IsPartitionEOF)
                        continue;

                    if (consumed.Message.Value is null)
                        continue;

                    using var scope = serviceScopeFactory.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<IProcessPaymentRequestHandler>();

                    handler.Handle(consumed.Message.Value.ToRequest());

                    consumer.Commit(consumed);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Could not consume payment request message");
                }

                await Task.Yield();
            }
        }
        finally
        {
            consumer.Close();
        }
    }
}