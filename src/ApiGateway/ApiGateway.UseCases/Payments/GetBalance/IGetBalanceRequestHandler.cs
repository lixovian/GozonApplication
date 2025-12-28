using ApiGateway.Entities.Models.Payments;

namespace ApiGateway.UseCases.Payments.GetBalance;

public interface IGetBalanceRequestHandler
{
    GetBalanceResponse Handle(GetBalanceApiRequest request);
}