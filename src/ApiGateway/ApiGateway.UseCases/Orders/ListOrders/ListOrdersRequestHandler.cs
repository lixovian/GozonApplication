using ApiGateway.Entities.Models.Orders;

namespace ApiGateway.UseCases.Orders.ListOrders;

internal sealed class ListOrdersRequestHandler : IListOrdersRequestHandler
{
    private readonly IOrderServiceClient _orderServiceClient;

    public ListOrdersRequestHandler(IOrderServiceClient orderServiceClient)
    {
        _orderServiceClient = orderServiceClient;
    }

    public ListOrdersResponse Handle(ListOrdersApiRequest request)
    {
        var orderDto = new ListOrdersRequest(request.UserId);
        return _orderServiceClient.ListOrders(orderDto);
    }
}