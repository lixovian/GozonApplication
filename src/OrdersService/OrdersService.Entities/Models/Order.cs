namespace OrdersService.Entities.Models;

public sealed class Order
{
    public Order(Guid id, int userId, decimal amount, string description, OrderStatus status)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Order id cannot be empty", nameof(id));

        if (userId < 0)
            throw new ArgumentException("User id cannot be less than 0", nameof(userId));

        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than 0", nameof(amount));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));

        Id = id;
        UserId = userId;
        Amount = amount;
        Description = description;
        Status = status;

        ValidateStatus();
    }

    public Guid Id { get; }
    public int UserId { get; }
    public decimal Amount { get; }
    public string Description { get; }
    public OrderStatus Status { get; }

    public Order MarkFinished()
    {
        EnsureCanChangeStatus(to: OrderStatus.Finished);
        return new Order(Id, UserId, Amount, Description, OrderStatus.Finished);
    }

    public Order MarkCancelled()
    {
        EnsureCanChangeStatus(to: OrderStatus.Cancelled);
        return new Order(Id, UserId, Amount, Description, OrderStatus.Cancelled);
    }

    private void ValidateStatus()
    {
        if (!Enum.IsDefined(typeof(OrderStatus), Status))
            throw new ArgumentOutOfRangeException(nameof(Status), Status, "Unknown order status");
    }

    private void EnsureCanChangeStatus(OrderStatus to)
    {
        if (Status == to)
            return;

        if (Status is OrderStatus.Finished or OrderStatus.Cancelled)
            throw new InvalidOperationException($"Cannot change status from terminal state '{Status}' to '{to}'");

        if (Status != OrderStatus.New)
            throw new InvalidOperationException($"Unsupported status transition from '{Status}' to '{to}'");
    }
}
