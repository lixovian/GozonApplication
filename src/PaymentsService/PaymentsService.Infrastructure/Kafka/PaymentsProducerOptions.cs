namespace PaymentsService.Infrastructure.Kafka;

public sealed class PaymentsProducerOptions
{
    public required string Topic { get; init; }
    public required string BootstrapServers { get; init; }
}