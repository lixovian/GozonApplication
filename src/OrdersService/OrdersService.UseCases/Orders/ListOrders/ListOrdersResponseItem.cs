namespace OrdersService.UseCases.Orders.ListOrders;

public sealed record ListOrdersResponseItem(
    Guid Id,
    int UserId,
    decimal Amount,
    string Description,
    string Status
);