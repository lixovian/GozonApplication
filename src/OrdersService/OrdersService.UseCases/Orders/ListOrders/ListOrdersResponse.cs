namespace OrdersService.UseCases.Orders.ListOrders;

public sealed record ListOrdersResponse(
    IReadOnlyList<ListOrdersResponseItem> Orders
);