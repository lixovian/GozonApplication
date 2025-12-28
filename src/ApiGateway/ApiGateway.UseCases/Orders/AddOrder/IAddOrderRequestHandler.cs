using ApiGateway.Entities.Models.Orders;

namespace ApiGateway.UseCases.Orders.AddOrder;

public interface IAddOrderRequestHandler
{
    AddOrderResponse Handle(AddOrderApiRequest apiRequest);
}