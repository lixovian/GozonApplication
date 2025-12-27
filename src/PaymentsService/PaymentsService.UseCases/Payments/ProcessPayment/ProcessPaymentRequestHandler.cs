namespace PaymentsService.UseCases.Payments.ProcessPayment;

internal sealed class ProcessPaymentRequestHandler : IProcessPaymentRequestHandler
{
    private readonly IProcessPaymentRepository _repository;
    private readonly TimeProvider _timeProvider;

    public ProcessPaymentRequestHandler(IProcessPaymentRepository repository, TimeProvider timeProvider)
    {
        _repository = repository;
        _timeProvider = timeProvider;
    }

    public ProcessPaymentResponse Handle(ProcessPaymentRequest request)
    {
        if (request.UserId <= 0)
            throw new ArgumentException("UserId must be greater than 0", nameof(request.UserId));

        if (request.OrderId == Guid.Empty)
            throw new ArgumentException("OrderId cannot be empty", nameof(request.OrderId));

        if (request.Amount <= 0)
            throw new ArgumentException("Amount must be greater than 0", nameof(request.Amount));

        if (string.IsNullOrWhiteSpace(request.Key))
            throw new ArgumentException("Key cannot be empty", nameof(request.Key));

        var now = _timeProvider.GetUtcNow();

        return _repository.Process(request, now);
    }
}