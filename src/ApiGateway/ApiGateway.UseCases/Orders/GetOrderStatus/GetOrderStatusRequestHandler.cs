using ApiGateway.Entities.Models.Orders;

namespace ApiGateway.UseCases.Orders.GetOrderStatus;

internal sealed class GetOrderStatusRequestHandler : IGetOrderStatusRequestHandler
{
    private readonly IOrderServiceClient _orderServiceClient;

    public GetOrderStatusRequestHandler(IOrderServiceClient orderServiceClient)
    {
        _orderServiceClient = orderServiceClient;
    }

    public GetOrderStatusResponse Handle(GetOrderStatusApiRequest request)
    {
        var orderDto = new GetOrderStatusRequest(request.UserId, request.OrderId);
        return _orderServiceClient.GetOrderStatus(orderDto);
    }
}