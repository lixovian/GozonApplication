using ApiGateway.Entities.Models.Orders;
using ApiGateway.UseCases.Orders.AddOrder;

namespace ApiGateway.UseCases.Orders;

public interface IOrderServiceClient
{
    AddOrderResponse CreateOrder(AddOrderRequest request);

    GetOrderStatusResponse GetOrderStatus(GetOrderStatusRequest request);

    ListOrdersResponse ListOrders(ListOrdersRequest request);
}