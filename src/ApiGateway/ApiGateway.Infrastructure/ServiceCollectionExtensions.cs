using ApiGateway.Infrastructure.Http;
using ApiGateway.UseCases.Orders;
using ApiGateway.UseCases.Payments;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiGateway.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient<IOrderServiceClient, OrderServiceClient>((sp, client) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();

            var baseUrl = configuration["OrdersApi:BaseUrl"];
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new InvalidOperationException("OrdersApi:BaseUrl is not configured.");

            client.BaseAddress = new Uri(baseUrl);
        });

        services.AddHttpClient<IPaymentsServiceClient, PaymentsServiceClient>((sp, client) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();

            var baseUrl = configuration["PaymentsApi:BaseUrl"];
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new InvalidOperationException("PaymentsApi:BaseUrl is not configured.");

            client.BaseAddress = new Uri(baseUrl);
        });

        return services;
    }
}