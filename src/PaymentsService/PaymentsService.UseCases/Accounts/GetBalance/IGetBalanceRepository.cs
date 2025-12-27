namespace PaymentsService.UseCases.Accounts.GetBalance;

public interface IGetBalanceRepository
{
    Guid? FindAccountId(int userId);

    decimal GetBalance(Guid accountId);
}