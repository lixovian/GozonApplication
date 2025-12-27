namespace PaymentsService.UseCases.Payments.ProcessPayment;

public interface IProcessPaymentRequestHandler
{
    ProcessPaymentResponse Handle(ProcessPaymentRequest request);
}