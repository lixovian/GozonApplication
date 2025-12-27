using Microsoft.Extensions.DependencyInjection;
using OrdersService.UseCases.Orders.AddOrder;
using OrdersService.UseCases.Orders.ApplyPaymentResult;
using OrdersService.UseCases.Orders.ListOrders;
using OrdersService.UseCases.Orders.GetOrderStatus;

namespace OrdersService.UseCases;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IAddOrderRequestHandler, AddOrderRequestHandler>();
        services.AddScoped<IListOrdersRequestHandler, ListOrdersRequestHandler>();
        services.AddScoped<IApplyPaymentResultRequestHandler, ApplyPaymentResultRequestHandler>();
        services.AddScoped<IGetOrderStatusRequestHandler, GetOrderStatusRequestHandler>();

        return services;
    }
}