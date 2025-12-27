namespace PaymentsService.Infrastructure.Outbox.Dtos;

internal sealed record OutboxMessage(Guid Id)
{
    public required string Type { get; init; }
    public required string Key { get; init; }
    public required string Payload { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? SentAt { get; set; }
}