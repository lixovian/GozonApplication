using ApiGateway.Entities.Models.Payments;

namespace ApiGateway.UseCases.Payments.AddAccount;

public interface IAddAccountRequestHandler
{
    AddAccountResponse Handle(AddAccountApiRequest request);
}