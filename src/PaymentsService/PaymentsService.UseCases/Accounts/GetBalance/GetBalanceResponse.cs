namespace PaymentsService.UseCases.Accounts.GetBalance;

public sealed record GetBalanceResponse(
    int UserId,
    Guid AccountId,
    decimal Balance
);