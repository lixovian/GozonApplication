using OrdersService.Infrastructure;
using OrdersService.Presentation.Endpoints;
using OrdersService.UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(TimeProvider.System); 

// OpenAPI
builder.Services.AddOpenApi("api");

// UseCases + Infrastructure
builder.Services.AddUseCases();
builder.Services.AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/api.json", "Orders API");
    });
}

app.UseHttpsRedirection();

app.MapOrdersEndpoints();

app.Run();