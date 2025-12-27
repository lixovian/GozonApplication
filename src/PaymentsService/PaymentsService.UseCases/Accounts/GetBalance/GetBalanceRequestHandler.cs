namespace PaymentsService.UseCases.Accounts.GetBalance;

internal sealed class GetBalanceRequestHandler : IGetBalanceRequestHandler
{
    private readonly IGetBalanceRepository _repository;

    public GetBalanceRequestHandler(IGetBalanceRepository repository)
    {
        _repository = repository;
    }

    public GetBalanceResponse Handle(GetBalanceRequest request)
    {
        if (request.UserId <= 0)
            throw new ArgumentException("UserId must be greater than 0", nameof(request.UserId));

        var accountId = _repository.FindAccountId(request.UserId);
        if (accountId is null)
            throw new InvalidOperationException($"Account for user {request.UserId} does not exist");

        var balance = _repository.GetBalance(accountId.Value);

        return new GetBalanceResponse(
            UserId: request.UserId,
            AccountId: accountId.Value,
            Balance: balance
        );
    }
}