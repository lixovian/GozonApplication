namespace ApiGateway.Entities.Models.Orders;
public sealed record AddOrderResponse(
    Guid Id,
    int UserId,
    decimal Amount,
    string Description,
    string Status
);