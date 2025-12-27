using ApiGateway.Presentation.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi("api");
builder.Services.AddSwaggerGen();

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/orders/openapi/api.json", "Orders API");
        options.SwaggerEndpoint("/payments/openapi/api.json", "Payments API");
    });
}

app.UseHttpsRedirection();

app.MapGatewayEndpoints();

app.MapReverseProxy();

app.Run();