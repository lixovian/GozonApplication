using ApiGateway.Entities.Models.Orders;

namespace ApiGateway.UseCases.Orders.ListOrders;

public interface IListOrdersRequestHandler
{
    ListOrdersResponse Handle(ListOrdersApiRequest request);
}