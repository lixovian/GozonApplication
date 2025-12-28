using System.Net.Http.Json;
using ApiGateway.Entities.Models.Payments;
using ApiGateway.UseCases.Payments;

namespace ApiGateway.Infrastructure.Http;

internal sealed class PaymentsServiceClient(HttpClient httpClient) : IPaymentsServiceClient
{
    public AddAccountResponse AddAccount(AddAccountRequest request)
    {
        var response = httpClient.PostAsJsonAsync("/payments/accounts", request)
            .GetAwaiter()
            .GetResult();

        if (!response.IsSuccessStatusCode)
        {
            var body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            throw new InvalidOperationException($"PaymentsService returned {(int)response.StatusCode}: {body}");
        }

        var result = response.Content.ReadFromJsonAsync<AddAccountResponse>()
            .GetAwaiter()
            .GetResult();

        if (result is null)
            throw new InvalidOperationException("PaymentsService returned empty response for AddAccount.");

        return result;
    }

    public GetBalanceResponse GetBalance(GetBalanceRequest request)
    {
        var url = $"/payments/balance?userId={request.UserId}";

        var result = httpClient.GetFromJsonAsync<GetBalanceResponse>(url)
            .GetAwaiter()
            .GetResult();

        if (result is null)
            throw new InvalidOperationException("PaymentsService returned empty response for GetBalance.");

        return result;
    }

    public TopUpAccountResponse TopUpAccount(TopUpAccountRequest request)
    {
        var response = httpClient.PostAsJsonAsync("/payments/accounts/top-up", request)
            .GetAwaiter()
            .GetResult();

        if (!response.IsSuccessStatusCode)
        {
            var body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            throw new InvalidOperationException($"PaymentsService returned {(int)response.StatusCode}: {body}");
        }

        var result = response.Content.ReadFromJsonAsync<TopUpAccountResponse>()
            .GetAwaiter()
            .GetResult();

        if (result is null)
            throw new InvalidOperationException("PaymentsService returned empty response for TopUpAccount.");

        return result;
    }
}
