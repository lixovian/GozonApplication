namespace PaymentsService.UseCases.Payments.ProcessPayment;

public sealed record ProcessPaymentResponse(
    Guid OrderId,
    string Key,
    string Result,
    string? Reason
);