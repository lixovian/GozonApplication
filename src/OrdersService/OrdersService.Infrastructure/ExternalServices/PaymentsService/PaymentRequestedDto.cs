namespace OrdersService.Infrastructure.ExternalServices.PaymentsService;

internal sealed record PaymentRequestedDto(
    Guid OrderId,
    int UserId,
    decimal Amount,
    string Description,
    string Key
);