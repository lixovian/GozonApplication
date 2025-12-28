using ApiGateway.Entities.Models.Orders;

namespace ApiGateway.UseCases.Orders.AddOrder;

internal sealed class AddOrderRequestHandler : IAddOrderRequestHandler
{
    private readonly IOrderServiceClient _orderServiceClient;

    public AddOrderRequestHandler(IOrderServiceClient orderServiceClient)
    {
        _orderServiceClient = orderServiceClient;
    }

    public AddOrderResponse Handle(AddOrderApiRequest apiRequest)
    {
        var orderDto = new AddOrderRequest(apiRequest.UserId, apiRequest.Amount, apiRequest.Description);
        return _orderServiceClient.CreateOrder(orderDto);
    }
}