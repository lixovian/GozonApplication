namespace ApiGateway.Entities.Models.Payments;
public sealed record TopUpAccountRequest(
    int UserId,
    decimal Amount,
    string Key
);