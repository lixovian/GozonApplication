namespace PaymentsService.UseCases.Payments.ProcessPayment;

public interface IProcessPaymentRepository
{
    ProcessPaymentResponse Process(ProcessPaymentRequest request, DateTimeOffset now);
}