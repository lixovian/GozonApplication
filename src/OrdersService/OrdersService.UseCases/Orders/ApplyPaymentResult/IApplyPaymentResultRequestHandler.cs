namespace OrdersService.UseCases.Orders.ApplyPaymentResult;

public interface IApplyPaymentResultRequestHandler
{
    void Handle(ApplyPaymentResultRequest request);
}