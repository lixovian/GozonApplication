using OrdersService.Entities.Models;
using OrdersService.UseCases.Orders.ListOrders;

namespace OrdersService.Infrastructure.Data.Orders;

internal sealed class ListOrdersRepository(OrdersDbContext dbContext)
    : IListOrdersRepository
{
    public IReadOnlyList<Order> GetAll(int userId)
    {
        return dbContext.Orders
            .Where(o => o.UserId == userId)
            .Select(o => o.ToEntity())
            .ToList();
    }
}