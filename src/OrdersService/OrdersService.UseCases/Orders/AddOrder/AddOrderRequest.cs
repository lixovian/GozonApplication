namespace OrdersService.UseCases.Orders.AddOrder;

public sealed record AddOrderRequest(
    int UserId,
    decimal Amount,
    string Description
);