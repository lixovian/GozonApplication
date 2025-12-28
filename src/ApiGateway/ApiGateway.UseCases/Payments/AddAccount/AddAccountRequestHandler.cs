using ApiGateway.Entities.Models.Payments;

namespace ApiGateway.UseCases.Payments.AddAccount;

internal sealed class AddAccountRequestHandler : IAddAccountRequestHandler
{
    private readonly IPaymentsServiceClient _paymentsServiceClient;

    public AddAccountRequestHandler(IPaymentsServiceClient paymentsServiceClient)
    {
        _paymentsServiceClient = paymentsServiceClient;
    }

    public AddAccountResponse Handle(AddAccountApiRequest request)
    {
        var paymentDto = new AddAccountRequest(request.UserId);
        return _paymentsServiceClient.AddAccount(paymentDto);
    }
}