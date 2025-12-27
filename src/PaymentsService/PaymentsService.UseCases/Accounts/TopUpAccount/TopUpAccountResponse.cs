namespace PaymentsService.UseCases.Accounts.TopUpAccount;

public sealed record TopUpAccountResponse(
    Guid AccountId,
    int UserId,
    decimal Amount,
    string Key,
    DateTimeOffset CreatedAt
);