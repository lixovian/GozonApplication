using OrdersService.Entities.Models;

namespace OrdersService.UseCases.Orders.ListOrders;

public interface IListOrdersRepository
{
    IReadOnlyList<Order> GetAll(int userId);
}