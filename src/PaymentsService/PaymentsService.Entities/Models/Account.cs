namespace PaymentsService.Entities.Models;

public sealed class Account
{
    public Account(Guid id, int userId, DateTimeOffset createdAt)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Account id cannot be empty", nameof(id));

        if (userId < 0)
            throw new ArgumentException("User id cannot be less than 0", nameof(userId));

        if (createdAt == default)
            throw new ArgumentException("CreatedAt must be set", nameof(createdAt));

        Id = id;
        UserId = userId;
        CreatedAt = createdAt;
    }

    public Guid Id { get; }
    public int UserId { get; }
    public DateTimeOffset CreatedAt { get; }

    public AccountTransaction CreateTopUp(decimal amount, string key, DateTimeOffset now)
    {
        return new AccountTransaction(
            id: Guid.NewGuid(),
            accountId: Id,
            type: TransactionType.TopUp,
            amount: amount,
            createdAt: now,
            key: key);
    }

    public AccountTransaction CreateDebit(decimal amount, string key, DateTimeOffset now)
    {
        return new AccountTransaction(
            id: Guid.NewGuid(),
            accountId: Id,
            type: TransactionType.Debit,
            amount: amount,
            createdAt: now,
            key: key);
    }
}