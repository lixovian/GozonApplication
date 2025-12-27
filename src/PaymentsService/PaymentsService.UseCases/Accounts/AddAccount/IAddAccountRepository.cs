using PaymentsService.Entities.Models;

namespace PaymentsService.UseCases.Accounts.AddAccount;

public interface IAddAccountRepository
{
    Account GetOrCreate(int userId, DateTimeOffset now);
}