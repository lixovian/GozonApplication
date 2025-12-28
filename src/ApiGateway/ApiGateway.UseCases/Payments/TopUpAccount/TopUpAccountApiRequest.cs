namespace ApiGateway.UseCases.Payments.TopUpAccount;

public sealed record TopUpAccountApiRequest(
    int UserId,
    decimal Amount,
    string Key
);