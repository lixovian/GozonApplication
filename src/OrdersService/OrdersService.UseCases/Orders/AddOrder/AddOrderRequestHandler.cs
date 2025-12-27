namespace OrdersService.UseCases.Orders.AddOrder;

internal sealed class AddOrderRequestHandler : IAddOrderRequestHandler
{
    private readonly IAddOrderRepository _repository;
    private readonly TimeProvider _timeProvider;

    public AddOrderRequestHandler(IAddOrderRepository repository, TimeProvider timeProvider)
    {
        _repository = repository;
        _timeProvider = timeProvider;
    }

    public AddOrderResponse Handle(AddOrderRequest request)
    {
        var (order, outbox) = request.ToEntity(_timeProvider);

        _repository.Add(order, outbox);

        return order.ToDto();
    }
}