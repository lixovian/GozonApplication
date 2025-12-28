using ApiGateway.Infrastructure;
using ApiGateway.Presentation.Endpoints;
using ApiGateway.UseCases;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi("api", options =>
{
    options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
});

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddOpenApi("api");

builder.Services.AddInfrastructure();
builder.Services.AddUseCases();

var app = builder.Build();

app.MapOpenApi();
app.UseCors("frontend");

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/api.json", "API Gateway");
});

app.MapApiGatewayEndpoints();

app.Run();