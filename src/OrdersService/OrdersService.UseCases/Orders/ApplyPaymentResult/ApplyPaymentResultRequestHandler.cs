namespace OrdersService.UseCases.Orders.ApplyPaymentResult;

internal sealed class ApplyPaymentResultRequestHandler(IApplyPaymentResultRepository repository)
    : IApplyPaymentResultRequestHandler
{
    public void Handle(ApplyPaymentResultRequest request)
    {
        var messageType = request.IsSuccess ? "PaymentSucceeded" : "PaymentFailed";

        repository.ApplyPaymentResult(
            orderId: request.OrderId,
            key: request.Key,
            isSuccess: request.IsSuccess,
            payloadJson: null,
            messageType: messageType);
    }
}