namespace ApiGateway.Entities.Models.Payments;
public sealed record TopUpAccountResponse(
    Guid AccountId,
    int UserId,
    decimal Amount,
    string Key,
    DateTimeOffset CreatedAt
);