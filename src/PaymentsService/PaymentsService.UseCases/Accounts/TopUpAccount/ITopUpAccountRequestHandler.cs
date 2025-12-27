namespace PaymentsService.UseCases.Accounts.TopUpAccount;

public interface ITopUpAccountRequestHandler
{
    TopUpAccountResponse Handle(TopUpAccountRequest request);
}