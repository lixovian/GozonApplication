namespace OrdersService.Infrastructure.Kafka;

public sealed class OrdersPaymentsResultsConsumerOptions
{
    public required string Topic { get; init; }
    public required string BootstrapServers { get; init; }
    public required string GroupId { get; set; } = "orders-service";
}