namespace OrdersService.UseCases.Orders.AddOrder;

public sealed record PaymentRequestedOutboxMessage(
    Guid OrderId,
    int UserId,
    decimal Amount,
    string Description,
    string Key,
    DateTimeOffset CreatedAt
);