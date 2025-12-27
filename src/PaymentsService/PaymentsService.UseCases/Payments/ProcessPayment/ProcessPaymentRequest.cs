namespace PaymentsService.UseCases.Payments.ProcessPayment;

public sealed record ProcessPaymentRequest(
    Guid OrderId,
    int UserId,
    decimal Amount,
    string Key
);