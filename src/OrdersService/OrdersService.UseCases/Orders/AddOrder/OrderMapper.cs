using OrdersService.Entities.Models;

namespace OrdersService.UseCases.Orders.AddOrder;

internal static class OrderMapper
{
    public static (Order Order, PaymentRequestedOutboxMessage Outbox) ToEntity(
        this AddOrderRequest request,
        TimeProvider timeProvider)
    {
        var now = timeProvider.GetUtcNow();

        var orderId = Guid.NewGuid();

        var order = new Order(
            id: orderId,
            userId: request.UserId,
            amount: request.Amount,
            description: request.Description,
            status: OrderStatus.New);

        var key = $"order-{orderId}";

        var outbox = new PaymentRequestedOutboxMessage(
            OrderId: orderId,
            UserId: request.UserId,
            Amount: request.Amount,
            Description: request.Description,
            Key: key,
            CreatedAt: now);

        return (order, outbox);
    }

    public static AddOrderResponse ToDto(this Order order) =>
        new AddOrderResponse(
            Id: order.Id,
            UserId: order.UserId,
            Amount: order.Amount,
            Description: order.Description,
            Status: order.Status.ToString().ToUpperInvariant()
        );
}