namespace OrdersService.UseCases.Orders.AddOrder;

public sealed record AddOrderResponse(
    Guid Id,
    int UserId,
    decimal Amount,
    string Description,
    string Status
);