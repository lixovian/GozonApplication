namespace OrdersService.UseCases.Orders.ListOrders;

internal sealed class ListOrdersRequestHandler(IListOrdersRepository repository)
    : IListOrdersRequestHandler
{
    public ListOrdersResponse Handle(int userId)
    {
        var orders = repository.GetAll(userId);

        return new ListOrdersResponse(
            orders.Select(OrderMapper.ToDto).ToList()
        );
    }
}