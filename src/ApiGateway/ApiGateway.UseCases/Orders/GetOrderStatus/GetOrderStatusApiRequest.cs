namespace ApiGateway.UseCases.Orders.GetOrderStatus;

public sealed record GetOrderStatusApiRequest(
    int UserId,
    Guid OrderId
);