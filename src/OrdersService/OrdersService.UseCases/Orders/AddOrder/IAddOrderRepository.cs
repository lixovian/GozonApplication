using OrdersService.Entities.Models;

namespace OrdersService.UseCases.Orders.AddOrder;

public interface IAddOrderRepository
{
    void Add(Order order, PaymentRequestedOutboxMessage outboxMessage);
}