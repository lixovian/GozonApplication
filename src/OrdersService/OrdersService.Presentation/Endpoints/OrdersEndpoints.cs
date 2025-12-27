using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using OrdersService.UseCases.Orders.AddOrder;
using OrdersService.UseCases.Orders.GetOrderStatus;
using OrdersService.UseCases.Orders.ListOrders;

namespace OrdersService.Presentation.Endpoints;

public static class OrdersEndpoints
{
    public static WebApplication MapOrdersEndpoints(this WebApplication app)
    {
        app.MapGroup("/orders")
            .WithTags("Orders")
            .MapAddOrder()
            .MapListOrders()
            .MapGetOrderStatus();

        return app;
    }

    private static RouteGroupBuilder MapAddOrder(this RouteGroupBuilder group)
    {
        group.MapPost("", (AddOrderRequest request, IAddOrderRequestHandler handler) =>
            {
                var response = handler.Handle(request);
                return Results.Ok(response);
            })
            .WithName("AddOrder")
            .WithSummary("Create a new order")
            .WithDescription("Creates order with status NEW and triggers async payment via outbox")
            .WithOpenApi();

        return group;
    }

    private static RouteGroupBuilder MapListOrders(this RouteGroupBuilder group)
    {
        group.MapGet("", (int userId, IListOrdersRequestHandler handler) =>
            {
                var response = handler.Handle(userId);
                return Results.Ok(response);
            })
            .WithName("ListOrders")
            .WithSummary("List orders")
            .WithDescription("Returns all orders for specified user")
            .WithOpenApi();

        return group;
    }


    private static RouteGroupBuilder MapGetOrderStatus(this RouteGroupBuilder group)
    {
        group.MapGet("{orderId:guid}/status",
                ([FromRoute] Guid orderId, [FromQuery] int userId, IGetOrderStatusRequestHandler handler) =>
                {
                    var response = handler.Handle(new GetOrderStatusRequest(userId, orderId));
                    return response is null ? Results.NotFound() : Results.Ok(response);
                })
            .WithName("GetOrderStatus")
            .WithSummary("Get order status")
            .WithDescription("Returns status for specified order of specified user")
            .WithOpenApi();

        return group;
    }
}
