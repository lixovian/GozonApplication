using PaymentsService.Infrastructure;
using PaymentsService.Presentation.Endpoints;
using PaymentsService.UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(TimeProvider.System); 

builder.Services.AddOpenApi(documentName: "api");

builder.Services.AddUseCases();
builder.Services.AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/api.json", "Payments API");
    });
}

app.UseHttpsRedirection();

app.MapPaymentsEndpoints();

app.Run();