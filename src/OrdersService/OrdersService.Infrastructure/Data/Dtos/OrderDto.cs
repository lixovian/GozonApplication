namespace OrdersService.Infrastructure.Data.Dtos;

public sealed record OrderDto(Guid Id)
{
    public int UserId { get; init; }
    public decimal Amount { get; init; }
    public string Description { get; init; } = string.Empty;

    public string Status { get; init; } = "NEW";
}