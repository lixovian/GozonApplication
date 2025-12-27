namespace OrdersService.UseCases.Orders.GetOrderStatus;

public sealed record GetOrderStatusRequest(
    int UserId,
    Guid OrderId
);