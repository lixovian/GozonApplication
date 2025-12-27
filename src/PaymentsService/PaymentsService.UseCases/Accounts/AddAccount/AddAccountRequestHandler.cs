namespace PaymentsService.UseCases.Accounts.AddAccount;

internal sealed class AddAccountRequestHandler : IAddAccountRequestHandler
{
    private readonly IAddAccountRepository _repository;
    private readonly TimeProvider _timeProvider;

    public AddAccountRequestHandler(IAddAccountRepository repository, TimeProvider timeProvider)
    {
        _repository = repository;
        _timeProvider = timeProvider;
    }

    public AddAccountResponse Handle(AddAccountRequest request)
    {
        if (request.UserId <= 0)
            throw new ArgumentException("UserId must be greater than 0", nameof(request.UserId));

        var now = _timeProvider.GetUtcNow();

        var account = _repository.GetOrCreate(request.UserId, now);

        return account.ToDto();
    }
}