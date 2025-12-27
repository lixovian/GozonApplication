namespace PaymentsService.UseCases.Accounts.TopUpAccount;

public sealed record TopUpAccountRequest(
    int UserId,
    decimal Amount,
    string Key
);