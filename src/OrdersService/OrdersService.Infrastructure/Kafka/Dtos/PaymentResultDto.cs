namespace OrdersService.Infrastructure.Kafka.Dtos;

internal sealed record PaymentResultDto
{
    public required Guid OrderId { get; init; }
    public required string Key { get; init; }

    // SUCCEEDED / FAILED
    public required string Result { get; init; }

    public string? Reason { get; init; }
}