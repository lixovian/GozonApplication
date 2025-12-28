namespace ApiGateway.UseCases.Orders.AddOrder;

public sealed record AddOrderApiRequest(
    int UserId,
    decimal Amount,
    string Description
);