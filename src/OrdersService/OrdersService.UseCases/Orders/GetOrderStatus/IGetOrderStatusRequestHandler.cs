namespace OrdersService.UseCases.Orders.GetOrderStatus;

public interface IGetOrderStatusRequestHandler
{
    GetOrderStatusResponse? Handle(GetOrderStatusRequest request);
}