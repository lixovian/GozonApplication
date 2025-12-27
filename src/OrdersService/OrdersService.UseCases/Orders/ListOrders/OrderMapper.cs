using OrdersService.Entities.Models;
using Riok.Mapperly.Abstractions;

namespace OrdersService.UseCases.Orders.ListOrders;

[Mapper]
internal static partial class OrderMapper
{
    internal static partial ListOrdersResponseItem ToDto(this Order order);

    private static string MapStatus(OrderStatus status) =>
        status.ToString().ToUpperInvariant();
}