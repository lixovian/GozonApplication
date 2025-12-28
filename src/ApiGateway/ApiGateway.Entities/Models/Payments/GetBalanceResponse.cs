namespace ApiGateway.Entities.Models.Payments;
public sealed record GetBalanceResponse(
    int UserId,
    Guid AccountId,
    decimal Balance
);