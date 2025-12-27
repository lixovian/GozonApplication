using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrdersService.Infrastructure.Kafka.Dtos;
using OrdersService.UseCases.Orders.ApplyPaymentResult;

namespace OrdersService.Infrastructure.Kafka;

internal sealed class PaymentsResultsConsumer(
    IConsumer<Ignore, PaymentResultDto?> consumer,
    ILogger<PaymentsResultsConsumer> logger,
    IServiceScopeFactory scopeFactory,
    IOptions<OrdersPaymentsResultsConsumerOptions> options) : BackgroundService
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

                    var dto = consumed.Message.Value;

                    var payloadJson = JsonSerializer.Serialize(dto);

                    using var scope = scopeFactory.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<IApplyPaymentResultRequestHandler>();
                    var repo = scope.ServiceProvider.GetRequiredService<IApplyPaymentResultRepository>();

                    var isSuccess = dto.Result.Trim().Equals("SUCCEEDED", StringComparison.OrdinalIgnoreCase);

                    handler.Handle(new ApplyPaymentResultRequest(dto.OrderId, dto.Key, isSuccess, dto.Reason));

                    repo.ApplyPaymentResult(dto.OrderId, dto.Key, isSuccess, payloadJson,
                        isSuccess ? "PaymentSucceeded" : "PaymentFailed");

                    consumer.Commit(consumed);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Could not consume payment result message");
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
