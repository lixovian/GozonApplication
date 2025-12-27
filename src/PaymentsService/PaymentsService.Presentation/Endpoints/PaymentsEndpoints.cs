using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PaymentsService.UseCases.Accounts.AddAccount;
using PaymentsService.UseCases.Accounts.GetBalance;
using PaymentsService.UseCases.Accounts.TopUpAccount;

namespace PaymentsService.Presentation.Endpoints;

public static class PaymentsEndpoints
{
    // === WebApplication extension ===
    extension(WebApplication application)
    {
        public WebApplication MapPaymentsEndpoints()
        {
            application
                .MapGroup(prefix: "/payments")
                .MapAddAccount()
                .MapTopUpAccount()
                .MapGetBalance();

            return application;
        }
    }

    // === RouteGroupBuilder extensions ===
    extension(RouteGroupBuilder builder)
    {
        private RouteGroupBuilder MapAddAccount()
        {
            builder.MapPost(
                    pattern: "accounts",
                    (AddAccountRequest request, IAddAccountRequestHandler handler) =>
                    {
                        var response = handler.Handle(request);
                        return Results.Ok(response);
                    })
                .WithName("AddAccount")
                .WithSummary("Create account")
                .WithDescription("Creates a payment account for specified user if it does not exist");

            return builder;
        }

        private RouteGroupBuilder MapTopUpAccount()
        {
            builder.MapPost(
                    pattern: "accounts/top-up",
                    (TopUpAccountRequest request, ITopUpAccountRequestHandler handler) =>
                    {
                        var response = handler.Handle(request);
                        return Results.Ok(response);
                    })
                .WithName("TopUpAccount")
                .WithSummary("Top up account")
                .WithDescription("Adds funds to user's payment account");

            return builder;
        }

        private RouteGroupBuilder MapGetBalance()
        {
            builder.MapGet("/balance",
                    (int userId, IGetBalanceRequestHandler handler, CancellationToken ct) =>
                    {
                        var response = handler.Handle(new GetBalanceRequest(userId));
                        return Results.Ok(response);
                    })
                .WithName("GetBalance")
                .WithSummary("Get account balance")
                .WithDescription("Returns current balance of user's payment account.")
                .WithOpenApi();
            
            return builder;
        }
    }
}
