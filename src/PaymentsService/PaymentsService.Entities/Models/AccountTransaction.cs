namespace PaymentsService.Entities.Models;

public sealed class AccountTransaction
{
    public AccountTransaction(
        Guid id,
        Guid accountId,
        TransactionType type,
        decimal amount,
        DateTimeOffset createdAt,
        string key)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Transaction id cannot be empty", nameof(id));

        if (accountId == Guid.Empty)
            throw new ArgumentException("Account id cannot be empty", nameof(accountId));

        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than 0", nameof(amount));

        if (createdAt == default)
            throw new ArgumentException("CreatedAt must be set", nameof(createdAt));

        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be empty", nameof(key));

        Id = id;
        AccountId = accountId;
        Type = type;
        Amount = amount;
        CreatedAt = createdAt;
        Key = key;
    }

    public Guid Id { get; }
    public Guid AccountId { get; }
    public TransactionType Type { get; }
    public decimal Amount { get; }
    public DateTimeOffset CreatedAt { get; }

    public string Key { get; }

    public decimal SignedAmount => Type switch
    {
        TransactionType.TopUp => Amount,
        TransactionType.Debit => -Amount,
        _ => throw new ArgumentOutOfRangeException()
    };
}