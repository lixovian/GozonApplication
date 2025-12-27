using Microsoft.OpenApi;
using OrdersService.Infrastructure;
using OrdersService.Presentation.Endpoints;
using OrdersService.UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi("api", options =>
{
    options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
});

builder.Services.AddSingleton(TimeProvider.System); 

builder.Services.AddOpenApi("api");

builder.Services.AddUseCases();
builder.Services.AddInfrastructure();

var app = builder.Build();

app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/api.json", "Orders API");
    });
}

app.UseHttpsRedirection();

app.MapOrdersEndpoints();

app.Run();