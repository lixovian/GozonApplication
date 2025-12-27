using OrdersService.Entities.Models;

namespace OrdersService.UseCases.Orders.GetOrderStatus;

public interface IGetOrderStatusRepository
{
    Order? GetById(int userId, Guid orderId);
}