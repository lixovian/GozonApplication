namespace OrdersService.UseCases.Orders.GetOrderStatus;

public sealed record GetOrderStatusResponse(
    Guid Id,
    int UserId,
    decimal Amount,
    string Description,
    string Status
);