using System.Net.Http.Json;
using ApiGateway.Entities.Models.Orders;
using ApiGateway.UseCases.Orders;
using ApiGateway.UseCases.Orders.GetOrderStatus;
using ApiGateway.UseCases.Orders.ListOrders;

namespace ApiGateway.Infrastructure.Http;

internal sealed class OrderServiceClient(HttpClient httpClient) : IOrderServiceClient
{
    public AddOrderResponse CreateOrder(AddOrderRequest request)
    {
        var response = httpClient.PostAsJsonAsync("/orders", request)
            .GetAwaiter()
            .GetResult();

        if (!response.IsSuccessStatusCode)
        {
            var body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            throw new InvalidOperationException($"OrdersService returned {(int)response.StatusCode}: {body}");
        }

        var result = response.Content.ReadFromJsonAsync<AddOrderResponse>()
            .GetAwaiter()
            .GetResult();

        if (result is null)
            throw new InvalidOperationException("OrdersService returned empty response for CreateOrder.");

        return result;
    }

    public GetOrderStatusResponse GetOrderStatus(GetOrderStatusRequest request)
    {
        var url = $"/orders/{request.OrderId:D}/status?userId={request.UserId}";

        var result = httpClient.GetFromJsonAsync<GetOrderStatusResponse>(url)
            .GetAwaiter()
            .GetResult();

        if (result is null)
            throw new InvalidOperationException("OrdersService returned empty response for GetOrderStatus.");

        return result;
    }

    public ListOrdersResponse ListOrders(ListOrdersRequest request)
    {
        var url = $"/orders?userId={request.UserId}";

        var result = httpClient.GetFromJsonAsync<ListOrdersResponse>(url)
            .GetAwaiter()
            .GetResult();

        if (result is null)
            throw new InvalidOperationException("OrdersService returned empty response for ListOrders.");

        return result;
    }
}
