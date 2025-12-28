using ApiGateway.Entities.Models.Orders;

namespace ApiGateway.UseCases.Orders.GetOrderStatus;

public interface IGetOrderStatusRequestHandler
{
    GetOrderStatusResponse Handle(GetOrderStatusApiRequest request);
}