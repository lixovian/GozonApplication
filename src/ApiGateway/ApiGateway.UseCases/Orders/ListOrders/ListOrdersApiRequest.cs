namespace ApiGateway.UseCases.Orders.ListOrders;

public sealed record ListOrdersApiRequest(
    int UserId
);