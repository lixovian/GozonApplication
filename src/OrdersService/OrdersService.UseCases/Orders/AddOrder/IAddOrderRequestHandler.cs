namespace OrdersService.UseCases.Orders.AddOrder;

public interface IAddOrderRequestHandler
{
    AddOrderResponse Handle(AddOrderRequest request);
}