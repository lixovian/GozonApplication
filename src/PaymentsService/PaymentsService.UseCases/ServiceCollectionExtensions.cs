using Microsoft.Extensions.DependencyInjection;
using PaymentsService.UseCases.Accounts.AddAccount;
using PaymentsService.UseCases.Accounts.GetBalance;
using PaymentsService.UseCases.Accounts.TopUpAccount;
using PaymentsService.UseCases.Payments.ProcessPayment;

namespace PaymentsService.UseCases;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);

        services.AddScoped<IAddAccountRequestHandler, AddAccountRequestHandler>();
        services.AddScoped<ITopUpAccountRequestHandler, TopUpAccountRequestHandler>();
        services.AddScoped<IGetBalanceRequestHandler, GetBalanceRequestHandler>();
        services.AddScoped<IProcessPaymentRequestHandler, ProcessPaymentRequestHandler>();
        return services;
    }
}