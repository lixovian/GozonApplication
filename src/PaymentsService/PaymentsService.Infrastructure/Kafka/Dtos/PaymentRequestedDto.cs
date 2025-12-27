namespace PaymentsService.Infrastructure.Kafka.Dtos;

internal sealed class PaymentRequestedDto
{
    public required Guid OrderId { get; init; }
    public required int UserId { get; init; }
    public required decimal Amount { get; init; }
    public required string Key { get; init; }
}