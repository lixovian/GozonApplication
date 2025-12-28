namespace ApiGateway.Entities.Models.Payments;
public sealed record AddAccountResponse(
    Guid Id,
    int UserId,
    DateTimeOffset CreatedAt
);