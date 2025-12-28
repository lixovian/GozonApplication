using ApiGateway.Entities.Models.Payments;

namespace ApiGateway.UseCases.Payments;

public interface IPaymentsServiceClient
{
    AddAccountResponse AddAccount(AddAccountRequest request);

    GetBalanceResponse GetBalance(GetBalanceRequest request);

    TopUpAccountResponse TopUpAccount(TopUpAccountRequest request);
}