namespace OrdersService.Infrastructure.ExternalServices.PaymentsService;

internal sealed class OrdersProducerOptions
{
    public required string Topic { get; init; }
    public required string BootstrapServers { get; init; }
}