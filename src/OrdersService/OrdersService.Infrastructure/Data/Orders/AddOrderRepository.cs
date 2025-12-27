using OrdersService.Entities.Models;
using OrdersService.Infrastructure.Data.Dtos;
using OrdersService.UseCases.Orders.AddOrder;

namespace OrdersService.Infrastructure.Data.Orders;

internal sealed class AddOrderRepository(OrdersDbContext dbContext) : IAddOrderRepository
{
    public void Add(Order order, PaymentRequestedOutboxMessage outboxMessage)
    {
        dbContext.Add(order.ToDto());

        var outboxDto = new OutboxMessageDto(Guid.NewGuid())
        {
            Type = "PaymentRequested",
            Key = outboxMessage.Key,
            Payload = System.Text.Json.JsonSerializer.Serialize(outboxMessage),
            CreatedAt = outboxMessage.CreatedAt,
            SentAt = null
        };

        dbContext.Add(outboxDto);

        dbContext.SaveChanges();
    }
}