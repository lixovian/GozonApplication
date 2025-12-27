namespace OrdersService.Infrastructure.Data.Dtos;

public sealed record InboxMessageDto(Guid Id)
{
    public required string Type { get; init; }
    public required string Key { get; init; }
    public required string Payload { get; init; }
    public required DateTimeOffset ReceivedAt { get; init; }
}