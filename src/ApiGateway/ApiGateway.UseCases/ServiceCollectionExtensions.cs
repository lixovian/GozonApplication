using ApiGateway.UseCases.Orders.AddOrder;
using ApiGateway.UseCases.Orders.GetOrderStatus;
using ApiGateway.UseCases.Orders.ListOrders;
using ApiGateway.UseCases.Payments.AddAccount;
using ApiGateway.UseCases.Payments.GetBalance;
using ApiGateway.UseCases.Payments.TopUpAccount;
using Microsoft.Extensions.DependencyInjection;

namespace ApiGateway.UseCases;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IAddOrderRequestHandler, AddOrderRequestHandler>();
        services.AddScoped<IGetOrderStatusRequestHandler, GetOrderStatusRequestHandler>();
        services.AddScoped<IListOrdersRequestHandler, ListOrdersRequestHandler>();
        
        services.AddScoped<IAddAccountRequestHandler, AddAccountRequestHandler>();
        services.AddScoped<IGetBalanceRequestHandler, GetBalanceRequestHandler>();
        services.AddScoped<ITopUpAccountRequestHandler, TopUpAccountRequestHandler>();
        
        return services;
    }
}