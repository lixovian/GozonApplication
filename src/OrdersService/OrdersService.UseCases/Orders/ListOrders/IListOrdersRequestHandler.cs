namespace OrdersService.UseCases.Orders.ListOrders;

public interface IListOrdersRequestHandler
{
    ListOrdersResponse Handle(int userId);
}