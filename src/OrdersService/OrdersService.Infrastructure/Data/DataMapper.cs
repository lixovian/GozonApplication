using OrdersService.Entities.Models;
using OrdersService.Infrastructure.Data.Dtos;
using Riok.Mapperly.Abstractions;

namespace OrdersService.Infrastructure.Data;

[Mapper]
internal static partial class DataMapper
{
    public static partial OrderDto ToDto(this Order entity);
    public static partial Order ToEntity(this OrderDto dto);

    private static string MapStatus(OrderStatus status) =>
        status.ToString().ToUpperInvariant();

    private static OrderStatus MapStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return OrderStatus.New;

        return status.Trim().ToUpperInvariant() switch
        {
            "NEW" => OrderStatus.New,
            "FINISHED" => OrderStatus.Finished,
            "CANCELLED" => OrderStatus.Cancelled,
            _ => throw new InvalidOperationException("Unknown order status: " + status)
        };
    }
}