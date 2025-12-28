using ApiGateway.Entities.Models.Payments;

namespace ApiGateway.UseCases.Payments.GetBalance;

internal sealed class GetBalanceRequestHandler : IGetBalanceRequestHandler
{
    private readonly IPaymentsServiceClient _paymentsServiceClient;

    public GetBalanceRequestHandler(IPaymentsServiceClient paymentsServiceClient)
    {
        _paymentsServiceClient = paymentsServiceClient;
    }

    public GetBalanceResponse Handle(GetBalanceApiRequest request)
    {
        var paymentDto = new GetBalanceRequest(request.UserId);
        return _paymentsServiceClient.GetBalance(paymentDto);
    }
}