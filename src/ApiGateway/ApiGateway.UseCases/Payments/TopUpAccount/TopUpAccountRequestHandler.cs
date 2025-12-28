using ApiGateway.Entities.Models.Payments;

namespace ApiGateway.UseCases.Payments.TopUpAccount;

internal sealed class TopUpAccountRequestHandler : ITopUpAccountRequestHandler
{
    private readonly IPaymentsServiceClient _paymentsServiceClient;

    public TopUpAccountRequestHandler(IPaymentsServiceClient paymentsServiceClient)
    {
        _paymentsServiceClient = paymentsServiceClient;
    }

    public TopUpAccountResponse Handle(TopUpAccountApiRequest request)
    {
        var paymentDto = new TopUpAccountRequest(request.UserId, request.Amount, request.Key);
        return _paymentsServiceClient.TopUpAccount(paymentDto);
    }
}