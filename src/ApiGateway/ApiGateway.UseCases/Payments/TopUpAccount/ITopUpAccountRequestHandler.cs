using ApiGateway.Entities.Models.Payments;

namespace ApiGateway.UseCases.Payments.TopUpAccount;

public interface ITopUpAccountRequestHandler
{
    TopUpAccountResponse Handle(TopUpAccountApiRequest request);
}