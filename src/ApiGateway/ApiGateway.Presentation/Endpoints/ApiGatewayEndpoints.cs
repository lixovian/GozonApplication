using ApiGateway.Entities.Models.Orders;
using ApiGateway.Entities.Models.Payments;
using ApiGateway.UseCases.Orders.AddOrder;
using ApiGateway.UseCases.Orders.GetOrderStatus;
using ApiGateway.UseCases.Orders.ListOrders;
using ApiGateway.UseCases.Payments.AddAccount;
using ApiGateway.UseCases.Payments.GetBalance;
using ApiGateway.UseCases.Payments.TopUpAccount;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ApiGateway.Presentation.Endpoints;

public static class ApiGatewayEndpoints
{
    public static WebApplication MapApiGatewayEndpoints(this WebApplication app)
    {
        app.MapGroup("/orders")
            .WithTags("Orders")
            .MapAddOrder()
            .MapListOrders()
            .MapGetOrderStatus();

        app.MapGroup("/payments")
            .WithTags("Payments")
            .MapAddAccount()
            .MapTopUpAccount()
            .MapGetBalance();

        return app;
    }

    // -------- Orders --------

    private static RouteGroupBuilder MapAddOrder(this RouteGroupBuilder group)
    {
        group.MapPost("",
                (AddOrderApiRequest request, IAddOrderRequestHandler handler) =>
                {
                    var response = handler.Handle(request);
                    return Results.Ok(response);
                })
            .WithName("AddOrder")
            .WithSummary("Create a new order (gateway)")
            .WithDescription("Gateway endpoint that creates an order in OrdersService.")
            .WithOpenApi();

        return group;
    }

    private static RouteGroupBuilder MapListOrders(this RouteGroupBuilder group)
    {
        group.MapGet("",
                (int userId, IListOrdersRequestHandler handler) =>
                {
                    var response = handler.Handle(new ListOrdersApiRequest(userId));
                    return Results.Ok(response);
                })
            .WithName("ListOrders")
            .WithSummary("List orders (gateway)")
            .WithDescription("Gateway endpoint that returns all orders for specified user from OrdersService.")
            .WithOpenApi();

        return group;
    }

    private static RouteGroupBuilder MapGetOrderStatus(this RouteGroupBuilder group)
    {
        group.MapGet("{orderId:guid}/status",
                (Guid orderId, int userId, IGetOrderStatusRequestHandler handler) =>
                {
                    var response = handler.Handle(new GetOrderStatusApiRequest(userId, orderId));
                    return Results.Ok(response);
                })
            .WithName("GetOrderStatus")
            .WithSummary("Get order status (gateway)")
            .WithDescription("Gateway endpoint that returns order status from OrdersService.")
            .WithOpenApi();

        return group;
    }

    // -------- Payments --------

    private static RouteGroupBuilder MapAddAccount(this RouteGroupBuilder group)
    {
        group.MapPost("accounts",
                (AddAccountApiRequest request, IAddAccountRequestHandler handler) =>
                {
                    var response = handler.Handle(request);
                    return Results.Ok(response);
                })
            .WithName("AddAccount")
            .WithSummary("Create account (gateway)")
            .WithDescription("Gateway endpoint that creates a payment account via PaymentsService.")
            .WithOpenApi();

        return group;
    }

    private static RouteGroupBuilder MapTopUpAccount(this RouteGroupBuilder group)
    {
        group.MapPost("accounts/top-up",
                (TopUpAccountApiRequest request, ITopUpAccountRequestHandler handler) =>
                {
                    var response = handler.Handle(request);
                    return Results.Ok(response);
                })
            .WithName("TopUpAccount")
            .WithSummary("Top up account (gateway)")
            .WithDescription("Gateway endpoint that tops up account via PaymentsService.")
            .WithOpenApi();

        return group;
    }

    private static RouteGroupBuilder MapGetBalance(this RouteGroupBuilder group)
    {
        group.MapGet("balance",
                (int userId, IGetBalanceRequestHandler handler) =>
                {
                    var response = handler.Handle(new GetBalanceApiRequest(userId));
                    return Results.Ok(response);
                })
            .WithName("GetBalance")
            .WithSummary("Get account balance (gateway)")
            .WithDescription("Gateway endpoint that returns current balance via PaymentsService.")
            .WithOpenApi();

        return group;
    }
}
