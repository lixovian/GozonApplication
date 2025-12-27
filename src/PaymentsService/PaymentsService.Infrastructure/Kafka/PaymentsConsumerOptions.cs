namespace PaymentsService.Infrastructure.Kafka;

public sealed class PaymentsConsumerOptions
{
    public required string Topic { get; init; }
    public required string BootstrapServers { get; init; }
    public required string GroupId { get; set; } = "payments-service";
}