using Microsoft.EntityFrameworkCore;
using OrdersService.Entities.Models;
using OrdersService.UseCases.Orders.GetOrderStatus;

namespace OrdersService.Infrastructure.Data.Orders;

internal sealed class GetOrderStatusRepository(OrdersDbContext dbContext) : IGetOrderStatusRepository
{
    public Order? GetById(int userId, Guid orderId)
    {
        var dto = dbContext.Orders
            .AsNoTracking()
            .SingleOrDefault(x => x.Id == orderId && x.UserId == userId);

        return dto?.ToEntity();
    }
}