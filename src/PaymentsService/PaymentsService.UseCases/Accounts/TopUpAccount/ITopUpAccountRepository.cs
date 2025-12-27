using PaymentsService.Entities.Models;

namespace PaymentsService.UseCases.Accounts.TopUpAccount;

public interface ITopUpAccountRepository
{
    Account? FindAccount(int userId);

    AccountTransaction AddTopUpTransaction(AccountTransaction transaction);
}