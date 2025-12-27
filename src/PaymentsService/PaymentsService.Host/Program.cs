using Microsoft.OpenApi;
using PaymentsService.Infrastructure;
using PaymentsService.Presentation.Endpoints;
using PaymentsService.UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi("api", options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Servers =
        [
            new OpenApiServer
            {
                Url = "/payments"
            }
        ];

        return Task.CompletedTask;
    });
    
    options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
});

builder.Services.AddSingleton(TimeProvider.System); 

builder.Services.AddOpenApi(documentName: "api");

builder.Services.AddUseCases();
builder.Services.AddInfrastructure();

var app = builder.Build();

app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/api.json", "Payments API");
    });
}


app.UseHttpsRedirection();

app.MapPaymentsEndpoints();

app.Run();