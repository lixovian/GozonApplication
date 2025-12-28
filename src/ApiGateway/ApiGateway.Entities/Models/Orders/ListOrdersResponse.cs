namespace ApiGateway.Entities.Models.Orders;
public sealed record ListOrdersResponse(
    IReadOnlyList<ListOrdersResponseItem> Orders
);