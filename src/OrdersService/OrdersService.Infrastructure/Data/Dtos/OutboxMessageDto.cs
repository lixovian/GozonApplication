namespace OrdersService.Infrastructure.Data.Dtos;

public sealed record OutboxMessageDto(Guid Id)
{
    public required string Type { get; init; }
    public required string Key { get; init; }
    public required string Payload { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? SentAt { get; set; }
}