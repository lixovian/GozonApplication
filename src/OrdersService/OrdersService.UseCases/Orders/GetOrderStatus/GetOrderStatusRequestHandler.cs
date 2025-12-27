namespace OrdersService.UseCases.Orders.GetOrderStatus;

internal sealed class GetOrderStatusRequestHandler(IGetOrderStatusRepository repository)
    : IGetOrderStatusRequestHandler
{
    public GetOrderStatusResponse? Handle(GetOrderStatusRequest request)
    {
        var order = repository.GetById(request.UserId, request.OrderId);
        return order is null ? null : order.ToDto();
    }
}