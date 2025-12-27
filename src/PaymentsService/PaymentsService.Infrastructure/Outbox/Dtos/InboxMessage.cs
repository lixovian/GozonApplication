namespace PaymentsService.Infrastructure.Outbox.Dtos;

internal sealed record InboxMessage(Guid Id)
{
    public required string Type { get; init; }
    public required string Key { get; init; }
    public required string Payload { get; init; }
    public required DateTimeOffset ReceivedAt { get; init; }
}

