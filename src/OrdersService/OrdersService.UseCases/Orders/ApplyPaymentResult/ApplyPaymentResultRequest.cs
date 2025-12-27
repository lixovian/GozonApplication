namespace OrdersService.UseCases.Orders.ApplyPaymentResult;

public sealed record ApplyPaymentResultRequest(
    Guid OrderId,
    string Key,
    bool IsSuccess,
    string? Reason
);