namespace ApiGateway.Entities.Models.Orders;
public sealed record GetOrderStatusResponse(
    Guid Id,
    int UserId,
    decimal Amount,
    string Description,
    string Status
);