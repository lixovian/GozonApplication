using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ApiGateway.Presentation.Endpoints;

public static class GatewayEndpoints
{
    public static WebApplication MapGatewayEndpoints(this WebApplication app)
    {
        // Тут можно добавить health endpoint
        app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
            .WithName("Health")
            .WithOpenApi();

        return app;
    }
}