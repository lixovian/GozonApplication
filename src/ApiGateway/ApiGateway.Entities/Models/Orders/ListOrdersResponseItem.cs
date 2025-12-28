namespace ApiGateway.Entities.Models.Orders;
public sealed record ListOrdersResponseItem(
    Guid Id,
    int UserId,
    decimal Amount,
    string Description,
    string Status
);