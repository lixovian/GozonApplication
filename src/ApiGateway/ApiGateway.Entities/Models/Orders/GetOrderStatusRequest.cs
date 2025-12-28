namespace ApiGateway.Entities.Models.Orders;
public sealed record GetOrderStatusRequest(
    int UserId,
    Guid OrderId
);