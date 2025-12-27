namespace PaymentsService.Infrastructure.Kafka.Dtos;

internal sealed class PaymentResultDto
{
    public required Guid OrderId { get; init; }
    public required string Key { get; init; }
    public required string Result { get; init; }
    public string? Reason { get; init; }
}