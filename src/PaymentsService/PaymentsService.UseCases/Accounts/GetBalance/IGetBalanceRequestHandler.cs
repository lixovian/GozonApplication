namespace PaymentsService.UseCases.Accounts.GetBalance;

public interface IGetBalanceRequestHandler
{
    GetBalanceResponse Handle(GetBalanceRequest request);
}