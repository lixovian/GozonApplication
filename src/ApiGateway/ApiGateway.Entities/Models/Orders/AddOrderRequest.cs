namespace ApiGateway.Entities.Models.Orders;
public sealed record AddOrderRequest(
    int UserId,
    decimal Amount,
    string Description
);