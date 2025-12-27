namespace PaymentsService.UseCases.Accounts.TopUpAccount;

internal sealed class TopUpAccountRequestHandler : ITopUpAccountRequestHandler
{
    private readonly ITopUpAccountRepository _repository;
    private readonly TimeProvider _timeProvider;

    public TopUpAccountRequestHandler(ITopUpAccountRepository repository, TimeProvider timeProvider)
    {
        _repository = repository;
        _timeProvider = timeProvider;
    }

    public TopUpAccountResponse Handle(TopUpAccountRequest request)
    {
        if (request.UserId <= 0)
            throw new ArgumentException("UserId must be greater than 0", nameof(request.UserId));

        if (request.Amount <= 0)
            throw new ArgumentException("Amount must be greater than 0", nameof(request.Amount));

        if (string.IsNullOrWhiteSpace(request.Key))
            throw new ArgumentException("Key cannot be empty", nameof(request.Key));

        var account = _repository.FindAccount(request.UserId);
        if (account is null)
            throw new InvalidOperationException($"Account for user {request.UserId} does not exist");

        var now = _timeProvider.GetUtcNow();

        var tx = account.CreateTopUp(request.Amount, request.Key, now);

        var savedTx = _repository.AddTopUpTransaction(tx);

        return Mapper.ToDto(savedTx, account);
    }
}