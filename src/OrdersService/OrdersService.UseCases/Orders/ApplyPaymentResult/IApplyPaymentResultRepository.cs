namespace OrdersService.UseCases.Orders.ApplyPaymentResult;

public interface IApplyPaymentResultRepository
{
    void ApplyPaymentResult(
        Guid orderId,
        string key,
        bool isSuccess,
        string? payloadJson,
        string messageType);
}