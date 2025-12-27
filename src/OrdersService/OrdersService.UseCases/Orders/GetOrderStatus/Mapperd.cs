using OrdersService.Entities.Models;
using Riok.Mapperly.Abstractions;

namespace OrdersService.UseCases.Orders.GetOrderStatus;

[Mapper]
internal static partial class Mapper
{
    internal static partial GetOrderStatusResponse ToDto(this Order order);

    private static string MapStatus(OrderStatus status) =>
        status.ToString().ToUpperInvariant();
}