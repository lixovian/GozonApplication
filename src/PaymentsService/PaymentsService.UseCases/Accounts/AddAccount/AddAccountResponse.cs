namespace PaymentsService.UseCases.Accounts.AddAccount;

public sealed record AddAccountResponse(
    Guid Id,
    int UserId,
    DateTimeOffset CreatedAt
);