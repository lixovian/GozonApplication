namespace PaymentsService.UseCases.Accounts.AddAccount;

public interface IAddAccountRequestHandler
{
    AddAccountResponse Handle(AddAccountRequest request);
}